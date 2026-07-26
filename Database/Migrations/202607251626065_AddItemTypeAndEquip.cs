namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItemTypeAndEquip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShopRedemptions", "IsEquiped", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShopItems", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.ShopItems", "ColorHex", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShopItems", "ColorHex");
            DropColumn("dbo.ShopItems", "Type");
            DropColumn("dbo.ShopRedemptions", "IsEquiped");
        }
    }
}
