using System;
using System.Collections.Generic;

namespace DriveLingo.Models
{
    public class DiscussionReply
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorRole { get; set; }
        public string AuthorAvatar { get; set; }
        public string Content { get; set; }
        public string DatePosted { get; set; }
        public bool IsEducatorAnswer { get; set; }
    }

    public class DiscussionThread
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorRole { get; set; }
        public string AuthorAvatar { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Content { get; set; }
        public string DatePosted { get; set; }
        public int Upvotes { get; set; }
        public List<string> UpvotedUserIds { get; set; }
        public List<DiscussionReply> Replies { get; set; }

        public DiscussionThread()
        {
            UpvotedUserIds = new List<string>();
            Replies = new List<DiscussionReply>();
        }
    }
}
