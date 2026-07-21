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
                    UserId = 1,
                    Title = "FIRST",
                    Content = "hahahahaha admin abuse for FIRST take that"
                }
            );
        }
    }
}