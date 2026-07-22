using System;

namespace DriveLingo.Models
{
    public class StoreItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Icon { get; set; }
        public string Category { get; set; }
    }

    public class Achievement
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int XpBonus { get; set; }
    }
}
