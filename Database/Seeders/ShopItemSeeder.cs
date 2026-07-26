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
                    Icon = "✨",
                    Name = "Diamond",
                    Description = "Shiny",
                    Type = ShopItem.ItemType.Border,
                    ColorHex = "Blue",
                    Cost = 100
                },
                new ShopItem
                {
                    Id = 2,
                    Icon = "🏎️",
                    Name = "Racing Driver Badge",
                    Description = "A racing driver themed profile image.",
                    Type = ShopItem.ItemType.Badge,
                    Cost = 1000
                },
                new ShopItem
                {
                    Id = 3,
                    Icon = "🌃",
                    Name = "Night Driver Avatar",
                    Description = "A neon night driving themed profile image.",
                    Type = ShopItem.ItemType.Icon,
                    Cost = 600
                },
                new ShopItem
                {
                    Id = 4,
                    Icon = "🏆",
                    Name = "Champion Driver Avatar",
                    Description = "A premium champion driver profile image.",
                    Type = ShopItem.ItemType.Icon,
                    Cost = 1000
                },
                new ShopItem
                {
                    Id = 5,
                    Icon = "⭕",
                    Name = "Basic Silver Border",
                    Description = "A simple silver profile border.",
                    Type = ShopItem.ItemType.Border,
                    ColorHex = "Silver",
                    Cost = 100
                },
                new ShopItem
                {
                    Id = 6,
                    Icon = "🌟",
                    Name = "Golden Border",
                    Description = "A premium golden profile border.",
                    Type = ShopItem.ItemType.Border,
                    ColorHex = "Gold",
                    Cost = 500
                },
                new ShopItem
                {
                    Id = 7,
                    Icon = "🚘",
                    Name = "Safe Driver Badge",
                    Description = "A badge awarded to users who value safe and responsible driving.",
                    Type = ShopItem.ItemType.Badge,
                    Cost = 500
                }
            );
        }
    }
}