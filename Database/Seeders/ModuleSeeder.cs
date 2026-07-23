using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DriveLingo.Database.Seeders
{
    using Models;
    public static class ModuleSeeder
    {
        public static void Run(AppDbContext context)
        {
            context.Modules.AddOrUpdate(
                new Module
                {
                    Id = 1,
                    Name = "Rules & Safety",
                    Description = "Essential Malaysian traffic regulations, speed limits, and road safety protocols.",
                    Lessons = new List<Lesson>
                    {
                        new Lesson
                        {
                            Title = "Malaysian Speed Limits Guidelines",
                            EstimatedTime = 5,
                            Image = "uploads/speed_limit_110.svg",
                            Pdf = "https://www.jpj.gov.my/documents/20124/0/KPP+Class+D+Manual.pdf",
                            Content = "Overview of Malaysian Speed Limits. Expressways (110 km/h), Federal/State Roads (90 km/h), Municipal/Town (60 km/h), School Zones (30 km/h)."
                        }
                    }
                },
                new Module
                {
                    Id = 2,
                    Name = "Road Signs",
                    Description = "Comprehensive guide to regulatory, warning, and informational road signs in Malaysia.",
                    Lessons = new List<Lesson>
                    {
                        new Lesson
                        {
                            Title = "Understanding Regulatory vs. Warning Signs",
                            EstimatedTime = 6,
                            Image = "uploads/no_entry.svg",
                            Pdf = null,
                            Content = "Regulatory Signs (Circular red border = prohibition / Stop sign = octagonal red), Warning Signs (Diamond yellow background), Informational Signs (Green = Expressway, Blue = Federal)."
                        }
                    }
                }
            );
        }
    }
}