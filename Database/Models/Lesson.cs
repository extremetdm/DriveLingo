using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }


        public int? EstimatedTime { get; set; }

        [StringLength(500)]
        public string Image { get; set; }

        [StringLength(500)]
        public string Pdf { get; set; }

        [Required]
        public int ModuleId { get; set; }

        [ForeignKey(nameof(ModuleId))]
        [InverseProperty("Lessons")]
        public virtual Module Module { get; set; }
        public virtual ICollection<CompletedLesson> CompletedUsers { get; set; }
    }

    public class CompletedLesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LessonId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(LessonId))]
        public virtual Lesson Lesson { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}