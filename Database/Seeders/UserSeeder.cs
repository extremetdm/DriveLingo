using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DriveLingo.Database.Seeders
{
    using Models;
    public static class UserSeeder
    {
        public static void Run(AppDbContext context)
        {
            context.Users.AddOrUpdate(
                u => u.Username,
                new User
                {
                    Role = User.UserRole.Admin,
                    Username = "admin",
                    Email = "admin@drivelingo.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"),
                    RegisteredAt = DateTime.Now
                }
            );
        }
    }
}