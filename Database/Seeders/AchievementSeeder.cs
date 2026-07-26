using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Seeders
{
    public static class AchievementSeeder
    {
        public static void Run(AppDbContext db)
        {
            db.Achievements.AddOrUpdate(
                new Achievement
                {
                    Id = 1,
                    Name = "First Step",
                    Description = "Complete your first driving quiz.",
                    Icon = "🎯",
                    Task = Achievement.TaskType.CompleteQuizzes,
                    Target = 1,
                    Xp = 50,
                    Points = 10
                },
                new Achievement
                {
                    Id = 2,
                    Name = "Quiz Master",
                    Description = "Complete 50 driving quizzes.",
                    Icon = "📚",
                    Task = Achievement.TaskType.CompleteQuizzes,
                    Target = 50,
                    Xp = 300,
                    Points = 100
                },
                new Achievement
                {
                    Id = 3,
                    Name = "Knowledge Seeker",
                    Description = "Read 20 driving lessons.",
                    Icon = "📖",
                    Task = Achievement.TaskType.ReadLessons,
                    Target = 20,
                    Xp = 200,
                    Points = 75
                },
                new Achievement
                {
                    Id = 4,
                    Name = "Perfect Driver",
                    Description = "Complete 10 perfect quizzes without mistakes.",
                    Icon = "⭐",
                    Task = Achievement.TaskType.CompletePerfectQuizzes,
                    Target = 10,
                    Xp = 500,
                    Points = 200
                },
                new Achievement
                {
                    Id = 5,
                    Name = "Shop Collector",
                    Description = "Redeem 5 items from the shop.",
                    Icon = "🛒",
                    Task = Achievement.TaskType.RedeemItems,
                    Target = 5,
                    Xp = 150,
                    Points = 50
                },
                new Achievement
                {
                    Id = 6,
                    Name = "Community Driver",
                    Description = "Create 10 posts in the driving forum.",
                    Icon = "💬",
                    Task = Achievement.TaskType.PostInForum,
                    Target = 10,
                    Xp = 250,
                    Points = 100
                },
                new Achievement
                {
                    Id = 7,
                    Name = "Road Legend",
                    Description = "Complete 100 driving quizzes and become a legendary learner.",
                    Icon = "🏆",
                    Task = Achievement.TaskType.CompleteQuizzes,
                    Target = 100,
                    Xp = 1000,
                    Points = 500
                }
            );
        }
    }
}