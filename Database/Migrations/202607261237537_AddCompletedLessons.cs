namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompletedLessons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompletedLessons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LessonId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.LessonId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompletedLessons", "UserId", "dbo.Users");
            DropForeignKey("dbo.CompletedLessons", "LessonId", "dbo.Lessons");
            DropIndex("dbo.CompletedLessons", new[] { "UserId" });
            DropIndex("dbo.CompletedLessons", new[] { "LessonId" });
            DropTable("dbo.CompletedLessons");
        }
    }
}
