using DriveLingo.Database.Models.Traits;
using DriveLingo.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DriveLingo.Database.Models
{
    public class User : Timestamps
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
        //[Index(IsUnique = true)]
        public string Username { get; set; }

        //[Required]
        [EmailAddress]
        [StringLength(256)]
        //[Index(IsUnique = true)]
        public string Email { get; set; }

        //[Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        public int XP { get; set; } = 0;

        [Required]
        public int Points { get; set; } = 0;

        public DateTime? RegisteredAt { get; set; }

        public virtual ICollection<ForumPost> ForumPosts { get; set; } = new HashSet<ForumPost> { };
        public virtual ICollection<ShopRedemption> ShopRedemptions { get; set; } = new HashSet<ShopRedemption> { };
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new HashSet<QuizAttempt> { };
        public virtual ICollection<CompletedAchievement> Achievements { get; set; } = new HashSet<CompletedAchievement> { };
        public virtual ICollection<AchievementProgress> AchievementProgress { get; set; } = new HashSet<AchievementProgress> { };
        public virtual ICollection<CompletedLesson> CompletedLessons { get; set; } = new HashSet<CompletedLesson> { };


        [NotMapped]
        public int CurrentLevel => LevelingService.CalculateCurrentLevel(XP);

        [NotMapped]
        public int XpProgress => LevelingService.CalculateXpProgress(XP);

        [NotMapped]
        public int NextLevelXpRequired => LevelingService.CalculateRequiredXP(CurrentLevel);

        [NotMapped]
        public string Avatar {
            get
            {
                switch (Role)
                {
                    case UserRole.Admin:
                        return "👑";
                    case UserRole.Instructor:
                        return "👨‍✈️";
                    default:
                        return "🚗";
                }
            }
        }
    }
}