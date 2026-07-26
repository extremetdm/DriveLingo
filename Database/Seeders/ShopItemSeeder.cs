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
                }
            );
            db.ShopItems.AddOrUpdate(
                new ShopItem
                {
                    Id = 1,
                    Icon = "✨", // change to img
                    Name = "Diamond",
                    Description = "Shiny",
                    Cost = 100
                }
            );
        }
    }
}