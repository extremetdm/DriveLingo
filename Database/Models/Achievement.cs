using DriveLingo.Database.Models.Traits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models
{
    public class Achievement
    {
        public enum TaskType
        {
            CompleteQuizzes = 1,
            ReadLessons = 2,
            RedeemItems = 3,
            CompletePerfectQuizzes = 4,
            PostInForum = 5,
            //ReachLevel = 6
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        public string Icon { get; set; }

        [Required]
        public TaskType Task { get; set; }

        [Required]
        public int Target { get; set; }

        [Required]
        public int Xp { get; set; }

        [Required]
        public int Points { get; set; }
    }

    public class AchievementProgress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Index("UserTask", Order = 1, IsUnique = true)]
        public int UserId { get; set; }

        [Required]
        [Index("UserTask", Order = 2, IsUnique = true)]
        public Achievement.TaskType Task { get; set; }

        [Required]
        public int Progress { get; set; } = 0;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }

    public class CompletedAchievement : Timestamps
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Index("UserAchievement", Order = 1, IsUnique = true)]
        public int UserId { get; set; }

        [Required]
        [Index("UserAchievement", Order = 2, IsUnique = true)]
        public int AchievementId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(AchievementId))]
        public virtual Achievement Achievement { get; set; }
    }
}