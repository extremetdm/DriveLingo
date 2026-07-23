namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditModule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lessons", "EstimatedTime", c => c.Int());
            AddColumn("dbo.Lessons", "Image", c => c.String(maxLength: 500));
            AddColumn("dbo.Lessons", "Pdf", c => c.String(maxLength: 500));
            AlterColumn("dbo.Lessons", "Content", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Lessons", "Content", c => c.String(nullable: false, maxLength: 256));
            DropColumn("dbo.Lessons", "Pdf");
            DropColumn("dbo.Lessons", "Image");
            DropColumn("dbo.Lessons", "EstimatedTime");
        }
    }
}
