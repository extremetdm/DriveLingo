using System;
using System.Collections.Generic;

namespace DriveLingo.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "admin", "educator", "learner", "guest"
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Points { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public List<string> Achievements { get; set; }
        public List<string> Inventory { get; set; }
        public List<string> ReadMaterials { get; set; }
        public List<string> CompletedQuizzes { get; set; }
        public string EquippedBorder { get; set; }
        public string EquippedBorderColor { get; set; } // Dynamic border color defined by admin
        public string EquippedIcon { get; set; }
        public string EquippedBadge { get; set; }
        public string JoinedDate { get; set; }

        public User()
        {
            Achievements = new List<string>();
            Inventory = new List<string>();
            ReadMaterials = new List<string>();
            CompletedQuizzes = new List<string>();
        }

        public string DisplayAvatar
        {
            get
            {
                if (!string.IsNullOrEmpty(EquippedIcon)) return EquippedIcon;
                if (Role == "admin") return "👑";
                if (Role == "educator" || Role == "instructor") return "👨‍✈️";
                return "🚗"; // Default Learner Icon
            }
        }
    }
}
