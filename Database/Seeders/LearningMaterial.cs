using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Seeders
{
    public static class LearningMaterial
    {
        public static void Run(AppDbContext db)
        {
            var lesson1 = new Lesson
            {
                Id = 1,
                ModuleId = 1,
                EstimatedTime = 45,
                Title = "Road Signs",
                Content = "This chapter shows type of signs in Malaysia.",
                Image = "/uploads/signs.svg",
                Pdf = "uploads/pdf/Malaysia Road Sign.pdf",
                
            };
            var lesson2 = new Lesson
            {
                Id = 2,
                ModuleId = 2,
                EstimatedTime = 60,
                Title = "Road Rules",
                Content = "This chapter indicate the road rules in Malaysia.",
                Image = "/uploads/Rules.svg",
                Pdf = "uploads/pdf/Rules.pdf",

            };
            var lesson3 = new Lesson
            {
                Id = 3,
                ModuleId = 3,
                EstimatedTime = 60,
                Title = "Safety & Demerit System",
                Content = "This chapter explain the safety and demerit system.",
                Image = "/uploads/Road_safety.svg",
                Pdf = "uploads/pdf/Road_Safety.pdf",

            };
            db.Lessons.AddOrUpdate(lesson1);
            db.Lessons.AddOrUpdate(lesson2);
            db.Lessons.AddOrUpdate(lesson3);
        }
    }

}