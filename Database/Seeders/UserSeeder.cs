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
                    Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                    RegisteredAt = DateTime.Now
                },
                new User
                {
                    Role = User.UserRole.Instructor,
                    Username = "instructor",
                    Email = "instructor@drivelingo.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("instructor"),
                    RegisteredAt = DateTime.Now
                },
                new User
                {
                    Role = User.UserRole.Learner,
                    Username = "learner",
                    Email = "learner@drivelingo.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("learner"),
                    RegisteredAt = DateTime.Now,
                    XP = 100,
                    Points = 500000
                },
                new User
                {
                    Role = User.UserRole.Learner,
                    Username = "Vincent",
                    Email = "vincent@drivelingo.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Vincent123"),
                    RegisteredAt = DateTime.Now,
                    XP = 100,
                    Points = 500
                },
                new User
                {
                Role = User.UserRole.Learner,
                    Username = "Jaywen",
                    Email = "jaywen@drivelingo.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Jaywen123"),
                    RegisteredAt = DateTime.Now,
                    XP = 100,
                    Points = 500
                },
                new User
                {
                Role = User.UserRole.Learner,
                    Username = "ZiHeng",
                    Email = "ziheng@drivelingo.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Ziheng123"),
                    RegisteredAt = DateTime.Now,
                    XP = 100,
                    Points = 500
                },
                new User
                {
                Role = User.UserRole.Learner,
                    Username = "DaoMian",
                    Email = "daomian@drivelingo.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Daomian123"),
                    RegisteredAt = DateTime.Now,
                    XP = 100,
                    Points = 500
                }
            );
        }
    }
}