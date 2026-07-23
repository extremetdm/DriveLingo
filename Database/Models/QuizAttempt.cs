using DriveLingo.Database.Models.Traits;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models
{
    public class QuizAttempt: Timestamps
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int QuizId { get; set; }


        public int Score { get; set; }

        public bool Passed { get; set; }

        public DateTime CompletedAt { get; set; }


        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<QuizAttemptAnswer> Answers { get; set; } = new HashSet<QuizAttemptAnswer>();
    }
}