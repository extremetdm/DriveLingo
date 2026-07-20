using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DriveLingo.Database;
using DriveLingo.Database.Models;

namespace DriveLingo.Services
{
    public static class AuthService
    {
        public static bool Login(string username, string password)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.Where(u => u.Username == username)
                    .FirstOrDefault();

                if (user == null) return false;

                return BCrypt.Net.BCrypt.Verify(password, user.Password);
            }
        }

        public static User CreateGuest()
        {
            var guest = new User
            {
                Role = User.UserRole.Guest
            };

            using (var db = new AppDbContext())
            {
                db.Users.Add(guest);
                db.SaveChanges();
            }
            return guest;
        }

        public static User Register(string username, string password, string email)
        {
            return Register(username, password, email, null);
        }

        public static User Register(string username, string password, string email, User guest)
        {
            using (var db = new AppDbContext())
            {
                if (guest == null) guest = new User();

                guest.Role = User.UserRole.Learner;
                guest.Username = username;
                guest.Password = BCrypt.Net.BCrypt.HashPassword(password);
                guest.Email = email;

                db.Users.AddOrUpdate(guest);
                db.SaveChanges();
                
                return guest;
            }
        }
    }
}