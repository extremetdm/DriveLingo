using DriveLingo.Database;
using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace DriveLingo.Services
{
    public static class ShopService
    {
        public static ServiceStatusOutput HandleRedeem(int userId, int itemId)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(userId);
                if (user == null)
                    return new ServiceStatusOutput(false, "No user found.");

                var item = db.ShopItems.Find(itemId);
                if (item == null)
                    return new ServiceStatusOutput(false, "Shop item does not exist.");

                if (user.Points < item.Cost) 
                    return new ServiceStatusOutput(false, "Insufficient points.");

                if (user.ShopRedemptions.Any(r => r.ItemId == item.Id))
                    return new ServiceStatusOutput(false, "Shop item already redeemed.");

                user.Points -=  item.Cost;
                db.ShopRedemptions.Add(
                    new ShopRedemption
                    {
                        UserId = user.Id,
                        ItemId = item.Id,
                    }
                );
                db.SaveChanges();

                return new ServiceStatusOutput(true, "Successfully redeemed item.");
            }
        }
    }
}