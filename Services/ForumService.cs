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
            using (var db = new AppDbContext())
            {
                return db.ForumPosts
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
        }

        public static ServiceStatusOutput AddReply(int userId, int postId, string content)
        {
            using (var db = new AppDbContext())
            {
                content = content?.Trim();
                if (content == null || string.IsNullOrEmpty(content))
                    return new ServiceStatusOutput
                    {
                        Success = false,
                        Message = "Content cannot be empty."
                    };

                var user = db.Users.Find(userId);
                if (user == null)
                    return new ServiceStatusOutput
                    {
                        Success = false,
                        Message = "User not found."
                    };

                var post = db.ForumPosts.Find(postId);
                if (post == null)
                    return new ServiceStatusOutput
                    {
                        Success = false,
                        Message = "Post not found."
                    };

                var reply = new ForumPost
                {
                    Title = null,
                    Content = content,
                };

                user.ForumPosts.Add(reply);
                post.Replies.Add(reply);

                db.SaveChanges();

                return new ServiceStatusOutput
                {
                    Success = true,
                    Message = "Your comment reply has been added!"
                };
            }
        }
    }
}