using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace DriveLingo.Database.Seeders
{
    using Models;

    public static class ShopItemSeeder
    {
        public static void Run(AppDbContext db)
        {
            db.ShopItems.AddOrUpdate(
                new ShopItem
                {
                    Id = 1,
                    Icon = "✨", // change to img
                    Name = "Diamond",
                    Description = "Shiny",
                    Cost = 100
                },
                new ShopItem
                {
                    Id = 2,
                    Icon = "🏎️",
                    Name = "Racing Driver Avatar",
                    Description = "A racing driver themed profile image.",
                    Cost = 500
                },
                new ShopItem
                {
                    Id = 3,
                    Icon = "🌃",
                    Name = "Night Driver Avatar",
                    Description = "A neon night driving themed profile image.",
                    Cost = 600
                },
                new ShopItem
                {
                    Id = 4,
                    Icon = "🏆",
                    Name = "Champion Driver Avatar",
                    Description = "A premium champion driver profile image.",
                    Cost = 1000
                },
                new ShopItem
                {
                    Id = 5,
                    Icon = "⭕",
                    Name = "Basic Silver Border",
                    Description = "A simple silver profile border.",
                    Cost = 100
                },
                new ShopItem
                {
                    Id = 6,
                    Icon = "🌟",
                    Name = "Golden Border",
                    Description = "A premium golden profile border.",
                    Cost = 500
                }
            );
        }
    }
}