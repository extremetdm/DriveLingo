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
        public string Category { get; set; } // "Border", "Icon", "Badge"
        public string ColorHex { get; set; } // Dynamic hex color defined by admin for Border items (e.g., "#6366f1", "#f59e0b")

        public StoreItem()
        {
            ColorHex = "#6366f1";
        }
    }

    public class Achievement
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int XpBonus { get; set; }
        public int TargetCount { get; set; }
        public string MetricType { get; set; }

        public Achievement()
        {
            TargetCount = 1;
            MetricType = "quiz_count";
        }
    }
}
