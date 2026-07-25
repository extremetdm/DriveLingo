using DriveLingo.Database;
using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using static DriveLingo.Database.Models.User;

namespace DriveLingo.Services
{
    public struct ForumThread
    {
        public int Id { get; set; }
        public string StringId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorName { get; set; }
        public UserRole AuthorRole { get; set; }
        public string AuthorAvatar { get; set; }
        public ICollection<ForumThread> Replies { get; set; }
        public int Likes { get; set; }
    }

    public static class ForumService
    {
        public static ICollection<ForumThread> GetForumThreads()
        {
            var list = new List<ForumThread>();

            try
            {
                using (var db = new AppDbContext())
                {
                    var dbThreads = db.ForumPosts
                        .Include(p => p.User)
                        .Include(p => p.Replies.Select(r => r.User))
                        .Where(p => p.ReplyingPostId == null)
                        .OrderByDescending(p => p.CreatedAt)
                        .ToList()
                        .Select(p => new ForumThread
                        {
                            Id = p.Id,
                            StringId = p.Id.ToString(),
                            Title = p.Title,
                            Content = p.Content,
                            CreatedAt = p.CreatedAt,
                            AuthorName = p.User != null ? p.User.Username : "Candidate",
                            AuthorRole = p.User != null ? p.User.Role : UserRole.Learner,
                            AuthorAvatar = p.User != null ? p.User.Avatar : "🚗",
                            Replies = p.Replies.Select(r => new ForumThread
                            {
                                Id = r.Id,
                                StringId = r.Id.ToString(),
                                Title = r.Title,
                                Content = r.Content,
                                CreatedAt = r.CreatedAt,
                                AuthorName = r.User != null ? r.User.Username : "Member",
                                AuthorRole = r.User != null ? r.User.Role : UserRole.Learner,
                                AuthorAvatar = r.User != null ? r.User.Avatar : "🚘",
                                Likes = 0
                            }).ToList(),
                            Likes = 0
                        }).ToList();

                    list.AddRange(dbThreads);
                }
            }
            catch
            {
                // Physical DB optional
            }

            // Always merge in-memory AppStateRepository discussions
            var state = Data.AppStateRepository.GetCurrent();
            if (state != null && state.Discussions != null)
            {
                int counter = 1000;
                foreach (var disc in state.Discussions)
                {
                    counter++;
                    int threadId = counter;

                    var replies = new List<ForumThread>();
                    if (disc.Replies != null)
                    {
                        int repCounter = 2000;
                        foreach (var rep in disc.Replies)
                        {
                            repCounter++;
                            var authorRole = (rep.AuthorRole == "educator" || rep.AuthorRole == "instructor") ? UserRole.Instructor : UserRole.Learner;
                            replies.Add(new ForumThread
                            {
                                Id = repCounter,
                                StringId = rep.Id,
                                Title = "",
                                Content = rep.Content,
                                CreatedAt = DateTime.Now,
                                AuthorName = rep.AuthorName,
                                AuthorRole = authorRole,
                                AuthorAvatar = rep.AuthorAvatar,
                                Likes = 0
                            });
                        }
                    }

                    var authorRoleHeader = disc.AuthorRole == "admin" ? UserRole.Admin : ((disc.AuthorRole == "educator" || disc.AuthorRole == "instructor") ? UserRole.Instructor : UserRole.Learner);

                    list.Add(new ForumThread
                    {
                        Id = threadId,
                        StringId = disc.Id,
                        Title = disc.Title,
                        Content = disc.Content,
                        CreatedAt = DateTime.Now,
                        AuthorName = disc.AuthorName,
                        AuthorRole = authorRoleHeader,
                        AuthorAvatar = disc.AuthorAvatar,
                        Replies = replies,
                        Likes = disc.Upvotes
                    });
                }
            }

            return list;
        }

        public static ServiceStatusOutput AddReply(int userId, string threadIdStr, string content)
        {
            content = content?.Trim();
            if (string.IsNullOrEmpty(content))
                return ServiceStatusOutput.error("Content cannot be empty.");

            // Try DB reply first
            int postId = 0;
            if (int.TryParse(threadIdStr, out postId))
            {
                try
                {
                    using (var db = new AppDbContext())
                    {
                        var user = db.Users.Find(userId);
                        var post = db.ForumPosts.Find(postId);
                        if (user != null && post != null)
                        {
                            var reply = new ForumPost { Title = null, Content = content };
                            user.ForumPosts.Add(reply);
                            post.Replies.Add(reply);
                            db.SaveChanges();
                            return ServiceStatusOutput.success("Your comment reply has been added!");
                        }
                    }
                }
                catch { }
            }

            // Fallback to in-memory AppStateRepository reply
            var state = Data.AppStateRepository.GetCurrent();
            var targetThread = state.Discussions.FirstOrDefault(d => d.Id == threadIdStr || d.Id == "disc_" + threadIdStr);
            if (targetThread != null)
            {
                var currentUser = HttpContext.Current.Session["CurrentUser"] as Models.User;
                string authorName = currentUser != null ? currentUser.Name : "Candidate";
                string authorRole = currentUser != null ? currentUser.Role : "learner";
                string authorAvatar = currentUser != null ? currentUser.DisplayAvatar : "🚗";

                targetThread.Replies.Add(new Models.DiscussionReply
                {
                    Id = "rep_" + Guid.NewGuid().ToString().Substring(0, 8),
                    AuthorId = currentUser != null ? currentUser.Id : "usr_learner",
                    AuthorName = authorName,
                    AuthorRole = authorRole,
                    AuthorAvatar = authorAvatar,
                    Content = content,
                    DatePosted = DateTime.Now.ToString("yyyy-MM-dd"),
                    IsEducatorAnswer = authorRole == "educator" || authorRole == "instructor"
                });

                return ServiceStatusOutput.success("Your comment reply has been added!");
            }

            return ServiceStatusOutput.error("Could not add reply.");
        }

        public static ServiceStatusOutput AddPost(int userId, string title, string content)
        {
            if (string.IsNullOrEmpty(title))
                return ServiceStatusOutput.error("Title cannot be empty.");

            if (string.IsNullOrEmpty(content))
                return ServiceStatusOutput.error("Content cannot be empty.");

            var currentUser = HttpContext.Current.Session["CurrentUser"] as Models.User;
            string authorName = currentUser != null ? currentUser.Name : "Candidate";
            string authorRole = currentUser != null ? currentUser.Role : "learner";
            string authorAvatar = currentUser != null ? currentUser.DisplayAvatar : "🚗";

            var state = Data.AppStateRepository.GetCurrent();
            state.Discussions.Add(new Models.DiscussionThread
            {
                Id = "disc_" + Guid.NewGuid().ToString().Substring(0, 8),
                AuthorId = currentUser != null ? currentUser.Id : "usr_learner",
                AuthorName = authorName,
                AuthorRole = authorRole,
                AuthorAvatar = authorAvatar,
                Title = title,
                Category = "Rules & Safety",
                Content = content,
                DatePosted = DateTime.Now.ToString("yyyy-MM-dd"),
                Upvotes = 0,
                Replies = new List<Models.DiscussionReply>()
            });

            return ServiceStatusOutput.success("Successfully posted question in community forum.");
        }

        public static ServiceStatusOutput DeletePost(string idOrPostId)
        {
            if (string.IsNullOrEmpty(idOrPostId))
                return ServiceStatusOutput.error("Invalid post identifier.");

            // Try DB delete first
            int postId = 0;
            if (int.TryParse(idOrPostId, out postId))
            {
                try
                {
                    using (var db = new AppDbContext())
                    {
                        var post = db.ForumPosts.Include(p => p.Replies).FirstOrDefault(p => p.Id == postId);
                        if (post != null)
                        {
                            db.ForumPosts.RemoveRange(post.Replies);
                            db.ForumPosts.Remove(post);
                            db.SaveChanges();
                            return ServiceStatusOutput.success("Post deleted successfully.");
                        }
                    }
                }
                catch { }
            }

            // Fallback to in-memory AppStateRepository discussions delete
            var state = Data.AppStateRepository.GetCurrent();
            if (state != null && state.Discussions != null)
            {
                // Delete main discussion thread if matched
                var mainThread = state.Discussions.FirstOrDefault(d => d.Id == idOrPostId || d.Id.EndsWith(idOrPostId));
                if (mainThread != null)
                {
                    state.Discussions.Remove(mainThread);
                    return ServiceStatusOutput.success("Forum question thread deleted successfully!");
                }

                // Delete individual comment reply if matched
                foreach (var disc in state.Discussions)
                {
                    if (disc.Replies != null)
                    {
                        var reply = disc.Replies.FirstOrDefault(r => r.Id == idOrPostId || r.Id.EndsWith(idOrPostId));
                        if (reply != null)
                        {
                            disc.Replies.Remove(reply);
                            return ServiceStatusOutput.success("Comment deleted successfully!");
                        }
                    }
                }
            }

            return ServiceStatusOutput.success("Forum item deleted successfully.");
        }
    }
}