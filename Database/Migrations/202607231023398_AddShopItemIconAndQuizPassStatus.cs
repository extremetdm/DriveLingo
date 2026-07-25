namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShopItemIconAndQuizPassStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShopItems", "Icon", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.QuizAttempts", "Passed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuizAttempts", "Passed");
            DropColumn("dbo.ShopItems", "Icon");
        }
    }
}
