using DriveLingo.Database;
using DriveLingo.Database.Models;
using DriveLingo.Services;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo
{
    public partial class Forum : AuthPage
    {
        public bool IsAdmin
        {
            get
            {
                var sessionUser = Session["CurrentUser"] as DriveLingo.Models.User;
                if (sessionUser != null && sessionUser.Role == "admin") return true;
                return CurrentUser != null && CurrentUser.Role == DriveLingo.Database.Models.User.UserRole.Admin;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();

            if (!IsPostBack)
            {
                BindForum();
            }
        }

        private void BindForum()
        {
            rptForum.DataSource = ForumService.GetForumThreads();
            rptForum.DataBind();
        }

        protected void rptForum_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var thread = (ForumThread)e.Item.DataItem;
            var rptReplies = (Repeater)e.Item.FindControl("rptReplies");

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

                PlaceHolder phEducatorReply = (PlaceHolder)e.Item.FindControl("phEducatorReply");
                PlaceHolder phStandardReply = (PlaceHolder)e.Item.FindControl("phStandardReply");

                bool isEducatorAnswer = reply.AuthorRole == DriveLingo.Database.Models.User.UserRole.Instructor;

                phEducatorReply.Visible = isEducatorAnswer;
                phStandardReply.Visible = !isEducatorAnswer;
            }
        }

        protected void btnToggleNewQuestion_Click(object sender, EventArgs e)
        {
            pnlNewQuestionForm.Visible = !pnlNewQuestionForm.Visible;
        }

        protected void btnPostQuestion_Click(object sender, EventArgs e)
        {
            if (CurrentUser == null) return;

            string title = txtForumTitle.Text.Trim();
            string content = txtForumContent.Text.Trim();

            var output = ForumService.AddPost(CurrentUser.Id, title, content);

            if (!output.Success)
            {
                ShowNotification(output.Message);
                return;
            }

            txtForumTitle.Text = "";
            txtForumContent.Text = "";
            pnlNewQuestionForm.Visible = false;

            ShowNotification("Your question has been posted to the community!");
            BindForum();
        }

        protected void rptForum_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Upvote")
            {
                BindForum();
            }
            else if (e.CommandName == "ReplyThread")
            {
                handleReply(source, e);
            }
            else if (e.CommandName == "DeleteThread")
            {
                string threadId = e.CommandArgument.ToString();
                var output = ForumService.DeletePost(threadId);
                ShowNotification(output.Message);
                BindForum();
            }
        }

        protected void rptReplies_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteReply")
            {
                string replyId = e.CommandArgument.ToString();
                var output = ForumService.DeletePost(replyId);
                ShowNotification(output.Message);
                BindForum();
            }
        }

        private void handleReply(object source, RepeaterCommandEventArgs e)
        {
            if (CurrentUser == null) return;

            string threadIdStr = e.CommandArgument.ToString();
            TextBox txtCandidateReply = (TextBox)e.Item.FindControl("txtCandidateReply");

            var output = ForumService.AddReply(CurrentUser.Id, threadIdStr, txtCandidateReply.Text);

            if (output.Success)
            {
                ShowNotification("Your comment reply has been added!");
                txtCandidateReply.Text = "";
                BindForum();
            }
            else
            {
                ShowNotification(output.Message);
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}