using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DriveLingo.Database.Seeders
{
    using Models;
    public static class ModuleSeeder
    {
        public static void Run(AppDbContext context)
        {
            context.Modules.AddOrUpdate(
                new Module
                {
                    Id = 1,
                    Name = "Road Signs",
                    Description = "You're not blind, so you better know what the damn symbols mean!"
                }
            );
        }
    }
}