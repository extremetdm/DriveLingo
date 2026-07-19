using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DriveLingo.Database.Migrations
{
    using Seeders;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Database\Migrations";
        }

        protected override void Seed(AppDbContext context)
        {
            UserSeeder.Run(context);

            context.SaveChanges();
        }
    }
}
