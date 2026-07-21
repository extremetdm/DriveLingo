using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models
{
    public class QuizAttemptAnswer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AttemptId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        // Nullable int to allow recording skipped/unanswered questions
        public int? ChoiceId { get; set; }

        [ForeignKey(nameof(AttemptId))]
        public virtual QuizAttempt QuizAttempt { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; }

        [ForeignKey(nameof(ChoiceId))]
        public virtual QuestionChoice Choice { get; set; }
    }
}