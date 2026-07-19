namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role = c.Int(nullable: false),
                        Username = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 256),
                        Password = c.String(nullable: false, maxLength: 100),
                        XP = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        RegisteredAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
