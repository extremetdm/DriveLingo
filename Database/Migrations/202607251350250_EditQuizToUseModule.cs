namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditQuizToUseModule : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Quizs", "LessonId", "dbo.Lessons");
            DropIndex("dbo.Quizs", new[] { "LessonId" });
            AddColumn("dbo.Quizs", "ModuleId", c => c.Int(nullable: false, defaultValue: 1));
            CreateIndex("dbo.Quizs", "ModuleId");
            AddForeignKey("dbo.Quizs", "ModuleId", "dbo.Modules", "Id", cascadeDelete: true);
            DropColumn("dbo.Quizs", "LessonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Quizs", "LessonId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Quizs", "ModuleId", "dbo.Modules");
            DropIndex("dbo.Quizs", new[] { "ModuleId" });
            DropColumn("dbo.Quizs", "ModuleId");
            CreateIndex("dbo.Quizs", "LessonId");
            AddForeignKey("dbo.Quizs", "LessonId", "dbo.Lessons", "Id", cascadeDelete: true);
        }
    }
}
