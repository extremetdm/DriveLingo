using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models
{
    public class Module
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; } = new HashSet<Lesson>();
    }
}