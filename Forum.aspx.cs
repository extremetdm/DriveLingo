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
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();

            if (!IsPostBack)
            {
                BindForum();
            }
        }

        struct ForumThread
        {
            public int Id { get; set; } 
            public string Title { get; set; }
            public string Content { get; set; }
            public DateTime CreatedAt { get; set; }
            public string AuthorName { get; set; }
            public UserRole AuthorRole { get; set; }
            public string AuthorAvatar { get; set; }
            public ICollection<ForumThread> Replies { get; set; }
            public int Likes { get; set; }

        }

        private void BindForum()
        {
            using (var db = new AppDbContext())
            {
                rptForum.DataSource = db.ForumPosts
                    .Include(p => p.User)
                    .Include(p => p.Replies.Select(r => r.User))
                    .Where(p => p.ReplyingPostId == null)
                    .OrderByDescending(p => p.CreatedAt)
                    .Select(p => new
                    {
                        p.Id,
                        p.Title,
                        p.Content,
                        p.CreatedAt,
                        Author = p.User,
                        Replies = p.Replies
                            .OrderBy(r => r.CreatedAt)
                            .Select(r => new
                            {
                                r.Id,
                                r.Title,
                                r.Content,
                                r.CreatedAt,
                                Author = r.User,
                                Likes = 0
                            }),
                            Likes = 0 // TODO CHANGE THIS
                    })
                    .ToList()
                    .Select(p => new ForumThread
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Content = p.Content,
                        CreatedAt = p.CreatedAt,
                        AuthorName = p.Author.Username,
                        AuthorRole = p.Author.Role,
                        AuthorAvatar = p.Author.Avatar,
                        Replies = p.Replies.Select(r => new ForumThread
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Content = r.Content,
                            CreatedAt = r.CreatedAt,
                            AuthorName = r.Author.Username,
                            AuthorRole = r.Author.Role,
                            AuthorAvatar = r.Author.Avatar,
                            Likes = 0
                        }).ToList(),
                        Likes = 0
                    })
                    .ToList();

            }
            rptForum.DataBind();
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

        // --- Forum Handlers ---
        protected void btnToggleNewQuestion_Click(object sender, EventArgs e)
        {
            pnlNewQuestionForm.Visible = !pnlNewQuestionForm.Visible;
        }

        protected void btnPostQuestion_Click(object sender, EventArgs e)
        {
            if (CurrentUser == null) return;

            string title = txtForumTitle.Text.Trim();
            //string category = ddlForumCategory.SelectedValue;
            string content = txtForumContent.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) return;

            using (var db = new AppDbContext())
            {

                db.ForumPosts.Add(new ForumPost
                {
                    UserId = CurrentUser.Id,
                    Title = title,
                    Content = content,
                    //Category = category,
                });
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
        }

        private void handleUpvote(object source, RepeaterCommandEventArgs e)
        {   //TODO CHANGE THIS

            //string threadId = e.CommandArgument.ToString();
            //var repo = AppStateRepository.GetCurrent();
            //var thread = repo.Discussions.FirstOrDefault(d => d.Id == threadId);
            //if (thread != null)
            //{
            //    thread.Upvotes++;
            //    BindForum();
            //}
            BindForum();
        }

        private void handleReply(object source, RepeaterCommandEventArgs e)
        {
            if (CurrentUser == null) return;

            TextBox txtCandidateReply = (TextBox)e.Item.FindControl("txtCandidateReply");


            if (txtCandidateReply == null || string.IsNullOrEmpty(txtCandidateReply.Text.Trim()))
                return;

            int postId = int.Parse(e.CommandArgument.ToString());

            using (var db = new AppDbContext())
            {
                var post = db.ForumPosts.Find(postId);
                if (post == null) return;

                post.Replies.Add(new ForumPost
                {
                    UserId = CurrentUser.Id,
                    Title = null,
                    Content = txtCandidateReply.Text.Trim(),
                });

                db.SaveChanges();

                ShowNotification("Your comment reply has been added!");
                txtCandidateReply.Text = "";
                BindForum();
            }
            
        }

        protected void rptForum_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item
                && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var thread = (ForumThread)e.Item.DataItem;
            Repeater rptReplies = (Repeater)e.Item.FindControl("rptReplies");

            if (rptReplies != null)
            {
                rptReplies.DataSource = thread.Replies;
                rptReplies.DataBind();
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
            // todo add error msg notification
        }
    }
}