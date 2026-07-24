using DriveLingo.Database.Models;
using DriveLingo.Models;
using DriveLingo.Services;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo.Instructor
{
    public partial class Forum : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Instructor);

            if (!IsPostBack)
            {
                BindForumForModeration();
            }
        }

        private void BindForumForModeration()
        {
            rptForumModeration.DataSource = ForumService.GetForumThreads();
            rptForumModeration.DataBind();
        }

        protected void rptForumModeration_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item
                && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var thread = (ForumThread)e.Item.DataItem;
            var rptReplies = (Repeater)e.Item.FindControl("rptEducatorReplies");

            if (rptReplies != null)
            {
                rptReplies.DataSource = thread.Replies;
                rptReplies.DataBind();
            }
        }

        protected void rptReplies_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var reply = (ForumThread)e.Item.DataItem;
                bool isEducatorAnswer = reply.AuthorRole == Database.Models.User.UserRole.Instructor;

                PlaceHolder phEducatorReply = (PlaceHolder)e.Item.FindControl("phEducatorReply");
                PlaceHolder phStandardReply = (PlaceHolder)e.Item.FindControl("phStandardReply");

                phEducatorReply.Visible = isEducatorAnswer;
                phStandardReply.Visible = !isEducatorAnswer;
            }
        }

        protected void rptForumModeration_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Reply")
            {
                if (CurrentUser == null) return;

                int postId = Convert.ToInt32(e.CommandArgument.ToString());
                TextBox txtReply = (TextBox)e.Item.FindControl("txtEducatorReply");

                var output = ForumService.AddReply(CurrentUser.Id, postId, txtReply.Text);

                if (output.Success)
                {
                    ShowNotification("Instructor verified response posted!");
                    txtReply.Text = "";
                    BindForumForModeration();
                }
                else
                {
                    ShowNotification(output.Message);
                }
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}