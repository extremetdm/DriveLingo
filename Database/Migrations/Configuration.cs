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
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"Database\Migrations";
        }

        protected override void Seed(AppDbContext context)
        {
            UserSeeder.Run(context);
            ModuleSeeder.Run(context);
            ShopItemSeeder.Run(context);
            QuizSeeder.Run(context);

            context.SaveChanges();

            ForumPostSeeder.Run(context);
            context.SaveChanges();
        }
    }
}
