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
        [StringLength(256)]
        public string Content { get; set; }

        [Required]
        public int ModuleId { get; set; }

        [ForeignKey(nameof(ModuleId))]
        [InverseProperty("Lessons")]
        public virtual Module Module { get; set; }
    }
}