using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LessonId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        // Navigation Properties
        public virtual ICollection<Question> Questions { get; set; } = new HashSet<Question>();
        public virtual ICollection<QuizAttempt> Attempts { get; set; } = new HashSet<QuizAttempt>();
    }
}