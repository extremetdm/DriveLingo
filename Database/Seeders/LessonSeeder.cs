using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DriveLingo.Database.Seeders
{
    using Models;
    public static class LessonSeeder
    {
        public static void Run(AppDbContext context)
        {
            context.Lessons.AddOrUpdate(
                new Lesson
                {
                    Id = 1,
                    ModuleId = 1,
                    Title = "Stop Sign",
                    Content = "This is a stop sign."
                }
            );
        }
    }
}