using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Models.Traits
{
    public class Timestamps
    {
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}