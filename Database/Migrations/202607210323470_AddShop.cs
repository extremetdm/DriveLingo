namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShop : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShopRedemptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShopItems", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.ShopItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 256),
                        Cost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "Points", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopRedemptions", "UserId", "dbo.Users");
            DropForeignKey("dbo.ShopRedemptions", "ItemId", "dbo.ShopItems");
            DropIndex("dbo.ShopRedemptions", new[] { "ItemId" });
            DropIndex("dbo.ShopRedemptions", new[] { "UserId" });
            DropColumn("dbo.Users", "Points");
            DropTable("dbo.ShopItems");
            DropTable("dbo.ShopRedemptions");
        }
    }
}
