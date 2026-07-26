using System.Data.Entity;
using DriveLingo.Database.Models;

namespace DriveLingo.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Disable cascade delete between Quiz and QuizAttempt
            modelBuilder.Entity<QuizAttempt>()
                .HasRequired(qa => qa.Quiz)
                .WithMany(q => q.Attempts)
                .HasForeignKey(qa => qa.QuizId)
                .WillCascadeOnDelete(false); 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionChoice> QuestionChoices { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<QuizAttemptAnswer> QuizAttemptAnswers { get; set; }

        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumLikes> ForumLikes { get; set; }

        public DbSet<ShopItem> ShopItems { get; set; }
        public DbSet<ShopRedemption> ShopRedemptions { get; set; }

        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<AchievementProgress> AchievementProgresses { get; set; }
        public DbSet<CompletedAchievement> CompletedAchievements { get; set; }

    }
}