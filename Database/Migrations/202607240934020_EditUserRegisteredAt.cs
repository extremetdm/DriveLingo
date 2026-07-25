namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUserRegisteredAt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "RegisteredAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "RegisteredAt", c => c.DateTime(nullable: false));
        }
    }
}
