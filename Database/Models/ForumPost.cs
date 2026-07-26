using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models
{
    using Traits;
    public class ForumPost: Timestamps
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("ForumPosts")]
        public virtual User User { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(256)]
        public string Content { get; set; }

        public int? ReplyingPostId { get; set; }

        [ForeignKey(nameof(ReplyingPostId))]
        [InverseProperty(nameof(Replies))]
        public virtual ForumPost ReplyingPost { get; set; }

        public virtual ICollection<ForumPost> Replies { get; set; } = new HashSet<ForumPost>();
        public virtual ICollection<ForumLikes> Likes { get; set; } = new HashSet<ForumLikes>();
    }

    public class ForumLikes
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public int PostId { get; set; }

        public virtual ForumPost Post { get; set; }
    }
}