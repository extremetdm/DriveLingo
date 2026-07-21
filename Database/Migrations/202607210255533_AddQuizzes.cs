namespace DriveLingo.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuizzes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionChoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        IsCorrect = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.QuizAttemptAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttemptId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        ChoiceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionChoices", t => t.ChoiceId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.QuizAttempts", t => t.AttemptId, cascadeDelete: true)
                .Index(t => t.AttemptId)
                .Index(t => t.QuestionId)
                .Index(t => t.ChoiceId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuizId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quizs", t => t.QuizId, cascadeDelete: true)
                .Index(t => t.QuizId);
            
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LessonId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuizAttempts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        QuizId = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CompletedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quizs", t => t.QuizId)
                .Index(t => t.QuizId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.QuizAttempts", "QuizId", "dbo.Quizs");
            DropForeignKey("dbo.QuizAttemptAnswers", "AttemptId", "dbo.QuizAttempts");
            DropForeignKey("dbo.QuestionChoices", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.QuizAttemptAnswers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.QuizAttemptAnswers", "ChoiceId", "dbo.QuestionChoices");
            DropIndex("dbo.QuizAttempts", new[] { "QuizId" });
            DropIndex("dbo.Questions", new[] { "QuizId" });
            DropIndex("dbo.QuizAttemptAnswers", new[] { "ChoiceId" });
            DropIndex("dbo.QuizAttemptAnswers", new[] { "QuestionId" });
            DropIndex("dbo.QuizAttemptAnswers", new[] { "AttemptId" });
            DropIndex("dbo.QuestionChoices", new[] { "QuestionId" });
            DropTable("dbo.QuizAttempts");
            DropTable("dbo.Quizs");
            DropTable("dbo.Questions");
            DropTable("dbo.QuizAttemptAnswers");
            DropTable("dbo.QuestionChoices");
        }
    }
}
