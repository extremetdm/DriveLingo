namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyToQuiz : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Quizs", "LessonId");
            AddForeignKey("dbo.Quizs", "LessonId", "dbo.Lessons", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quizs", "LessonId", "dbo.Lessons");
            DropIndex("dbo.Quizs", new[] { "LessonId" });
        }
    }
}
