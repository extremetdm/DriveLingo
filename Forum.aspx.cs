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
using static DriveLingo.Database.Models.User;

namespace DriveLingo
{
    public partial class Forum : AuthPage
    {
        public bool IsAdmin
        {
            get
            {
                var sessionUser = CurrentUser;
                return CurrentUser != null
                    && CurrentUser.Role == UserRole.Admin;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnToggleNewQuestion.Visible = !IsGuest;

            if (!IsPostBack)
            {
                BindForum();
            }
        }

        private void BindForum()
        {
            rptForum.DataSource = ForumService.GetForumThreads(CurrentUser?.Id);
            rptForum.DataBind();
        }

        protected void rptForum_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item
                && e.Item.ItemType != ListItemType.AlternatingItem)
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

                bool isEducatorAnswer = reply.AuthorRole == UserRole.Instructor;

                phEducatorReply.Visible = isEducatorAnswer;
                phStandardReply.Visible = !isEducatorAnswer;
            }
        }

        protected void btnToggleNewQuestion_Click(object sender, EventArgs e)
        {
            if (IsGuest)
            {
                ShowNotification("🔍 Guest Mode: Please register an account to make a post!");
                return;
            }

            pnlNewQuestionForm.Visible = !pnlNewQuestionForm.Visible;
        }

        protected void btnPostQuestion_Click(object sender, EventArgs e)
        {
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
                handleUpvote(source, e);
            }
            else if (e.CommandName == "ReplyThread")
            {
                handleReply(source, e);
            }
            else if (e.CommandName == "DeleteThread")
            {
                handleDelete(source, e);
            }
        }

        private void handleUpvote(object source, RepeaterCommandEventArgs e)
        {
            if (IsGuest)
            {
                ShowNotification("🔍 Guest Mode: Please register an account to like posts!");
                return;
            }

            int postId;
            if (!int.TryParse(e.CommandArgument.ToString(), out postId))
            {
                ShowNotification("Invalid post.");
                return;
            }

            var output = ForumService.ToggleLike(postId, CurrentUser.Id);
            if (output.Success)
            {
                BindForum();
            } else
            {
                ShowNotification(output.Message);
            }

        }

        private void handleReply(object source, RepeaterCommandEventArgs e)
        {
            if (IsGuest)
            {
                ShowNotification("🔍 Guest Mode: Please register an account to reply to posts!");
                return;
            }

            int postId;
            if (!int.TryParse(e.CommandArgument.ToString(), out postId))
            {
                ShowNotification("Invalid post.");
                return;
            }


            TextBox txtCandidateReply = (TextBox)e.Item.FindControl("txtCandidateReply");

            var output = ForumService.AddReply(CurrentUser.Id, postId, txtCandidateReply.Text);

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

        private void handleDelete(object source, RepeaterCommandEventArgs e)
        {
            int postId;
            if (!int.TryParse(e.CommandArgument.ToString(), out postId))
            {
                return;
            }

            var output = ForumService.DeletePost(postId);
            ShowNotification(output.Message);
            if (output.Success) BindForum();
        }


        protected void rptReplies_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteReply")
            {
                handleDelete(source, e);
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}