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
        public bool IsLiked { get; set; }
    }

    public static class ForumService
    {
        public static ICollection<ForumThread> GetForumThreads(int? userId)
        {
            using (var db = new AppDbContext())
            {
                return db.ForumPosts
                    .Include(p => p.User)
                    .Include(p => p.Likes)
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
                        Likes = p.Likes.Count,
                        IsLiked = p.Likes.Any(l => l.UserId == userId)
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
                        Likes = p.Likes,
                        IsLiked = p.IsLiked
                    })
                    .ToList();
            }
        }

        public static ServiceStatusOutput AddReply(int userId, int postId, string content)
        {
            content = content?.Trim();
            if (string.IsNullOrEmpty(content))
                return ServiceStatusOutput.error("Content cannot be empty.");

            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(userId);
                if (user == null)
                    return ServiceStatusOutput.error("User not found.");

                var post = db.ForumPosts.Find(postId);
                if (post == null)
                    return ServiceStatusOutput.error("Post not found.");

                var reply = new ForumPost
                {
                    Title = null,
                    Content = content,
                };

                user.ForumPosts.Add(reply);
                post.Replies.Add(reply);

                db.SaveChanges();

                return ServiceStatusOutput.success("Your comment reply has been added!");
            }
        }

        // todo potentially add category
        public static ServiceStatusOutput AddPost(int userId, string title, string content)
        {
            if (string.IsNullOrEmpty(title))
                return ServiceStatusOutput.error("Title cannot be empty.");

            if (string.IsNullOrEmpty(content))
                return ServiceStatusOutput.error("Content cannot be empty.");

            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(userId);
                if (user == null) return ServiceStatusOutput.error("User not found.");

                user.ForumPosts.Add(new ForumPost
                {
                    Title = title,
                    Content = content,
                    //Category = category,
                });

                var output = AchievementService.IncrementProgress(db, user, Achievement.TaskType.PostInForum);

                if (!output.Success)
                {
                    return output;
                }

                db.SaveChanges();

                return ServiceStatusOutput.success("Successfully posted in forum.", output.UnlockedAchievements);
            }
        }

        public static ServiceStatusOutput DeletePost(int postId)
        {
            using (var db = new AppDbContext())
            {
                var post = db.ForumPosts.Find(postId);
                if (post == null)
                    return ServiceStatusOutput.error("Invalid post identifier.");

                db.ForumPosts.RemoveRange(post.Replies);
                db.ForumPosts.Remove(post);
                db.SaveChanges();
                return ServiceStatusOutput.success("Post deleted successfully.");
            }
        }

        public static ServiceStatusOutput ToggleLike(int postId, int userId)
        {
            using (var db = new AppDbContext())
            {
                var post = db.ForumPosts.Find(postId);
                if (post == null)
                    return ServiceStatusOutput.error("Invalid post identifier.");

                var user = db.Users.Find(userId);
                if (user == null)
                    return ServiceStatusOutput.error("Invalid user.");

                var like = db.ForumLikes.FirstOrDefault(l => l.UserId == userId && l.PostId == postId);
                if (like == null)
                {
                    db.ForumLikes.Add(new ForumLikes
                    {
                        UserId = user.Id,
                        PostId = postId,
                    });
                    db.SaveChanges();
                    return ServiceStatusOutput.success("Post liked successfully.");
                }
                else
                {
                    db.ForumLikes.Remove(like);
                    db.SaveChanges();
                    return ServiceStatusOutput.success("Post unliked successfully.");
                }
            }
        }
    }
}