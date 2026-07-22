using System;
using System.Collections.Generic;

namespace DriveLingo.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "admin", "educator", "learner"
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Points { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public List<string> Achievements { get; set; }
        public List<string> Inventory { get; set; }
        public string EquippedBorder { get; set; }
        public string JoinedDate { get; set; }

        public User()
        {
            Achievements = new List<string>();
            Inventory = new List<string>();
        }
    }
}
