using System;
using System.ComponentModel.DataAnnotations;

namespace DriveLingo.Database.Models
{
    public class User
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

        [Required]
        [StringLength(100)]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        public int XP { get; set; } = 0;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime RegisteredAt { get; set; }

    }
}