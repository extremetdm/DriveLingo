namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditQuiz : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.QuizAttempts", "UserId");
            AddForeignKey("dbo.QuizAttempts", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuizAttempts", "UserId", "dbo.Users");
            DropIndex("dbo.QuizAttempts", new[] { "UserId" });
        }
    }
}
