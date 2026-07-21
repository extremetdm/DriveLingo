using DriveLingo.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DriveLingo.Database.Models
{
    public class User
    {
        public enum UserRole
        {
            Admin = 1,
            Instructor = 2,
            Learner = 3,
            Guest = 4
        }

        public int Id { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Username { get; set; }

        //[Required]
        [EmailAddress]
        [StringLength(256)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        //[Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        public int XP { get; set; } = 0;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime RegisteredAt { get; set; }

        public virtual ICollection<ForumPost> ForumPosts { get; set; } = new HashSet<ForumPost> { };

        [NotMapped]
        public int CurrentLevel => LevelingService.CalculateCurrentLevel(XP);
    }
}