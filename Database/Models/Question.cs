using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public int QuizId { get; set; }

        [Required]
        public string Text { get; set; }

        public string Image { get; set; }

        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<QuestionChoice> Choices { get; set; } = new HashSet<QuestionChoice>();
        public virtual ICollection<QuizAttemptAnswer> AttemptAnswers { get; set; } = new HashSet<QuizAttemptAnswer>();
    }
}