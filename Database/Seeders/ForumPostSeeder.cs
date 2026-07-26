using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Seeders
{
    public static class ForumPostSeeder
    {
        public static void Run(AppDbContext db)
        {
            db.ForumPosts.AddOrUpdate(
                new ForumPost
                {
                    Id = 1,
                    UserId = 3, 
                    Title = "Tips for Passing JPJ Undang Test",
                    Content = "I will be taking my JPJ Undang test soon. Does anyone have any tips on which topics I should focus on during revision?"
                },
                new ForumPost
                {
                    Id = 2,
                    UserId = 2, 
                    ReplyingPostId = 1,
                    Content = "Focus on road signs, traffic rules, speed limits, and common offences. These topics usually appear frequently in the JPJ Undang test."
                },
                new ForumPost
                {
                    Id = 3,
                    UserId = 4, 
                    ReplyingPostId = 1,
                    Content = "Practice with online JPJ question sets repeatedly. It helps you understand the question patterns and improve your confidence."
                },


                new ForumPost
                {
                    Id = 4,
                    UserId = 5, 
                    Title = "How to Remember Road Signs Easily?",
                    Content = "I always have difficulty remembering the meaning of different road signs. Are there any effective methods to memorize them?"
                },
                new ForumPost
                {
                    Id = 5,
                    UserId = 2, 
                    ReplyingPostId = 4,
                    Content = "Try grouping road signs by category such as warning signs, prohibition signs, and information signs. Understanding the meaning is better than memorizing randomly."
                },


                new ForumPost
                {
                    Id = 6,
                    UserId = 6, 
                    Title = "Common Mistakes During JPJ Undang Exam",
                    Content = "What are the common mistakes learners make during the JPJ Undang exam? I want to avoid making the same mistakes."
                },
                new ForumPost
                {
                    Id = 7,
                    UserId = 7, 
                    ReplyingPostId = 6,
                    Content = "Many learners rush through questions without reading carefully. Take your time and review your answers before submitting."
                }
            );
        }
    }
}