namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAchievements : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AchievementProgresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Task = c.Int(nullable: false),
                        Progress = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => new { t.UserId, t.Task }, unique: true, name: "UserTask");
            
            CreateTable(
                "dbo.CompletedAchievements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        AchievementId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Achievements", t => t.AchievementId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => new { t.UserId, t.AchievementId }, unique: true, name: "UserAchievement");
            
            CreateTable(
                "dbo.Achievements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 100),
                        Icon = c.String(nullable: false),
                        Task = c.Int(nullable: false),
                        Target = c.Int(nullable: false),
                        Xp = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompletedAchievements", "UserId", "dbo.Users");
            DropForeignKey("dbo.CompletedAchievements", "AchievementId", "dbo.Achievements");
            DropForeignKey("dbo.AchievementProgresses", "UserId", "dbo.Users");
            DropIndex("dbo.CompletedAchievements", "UserAchievement");
            DropIndex("dbo.AchievementProgresses", "UserTask");
            DropTable("dbo.Achievements");
            DropTable("dbo.CompletedAchievements");
            DropTable("dbo.AchievementProgresses");
        }
    }
}
