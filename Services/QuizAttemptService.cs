using DriveLingo.Database;
using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveLingo.Services
{
    public static class QuizAttemptService
    {
        public struct QuizAttemptResults
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public bool? Passed { get; set; }
            public int? Score { get; set; }
            public int? MaxScore { get; set; }
            public int? Percentage { get; set; }
            public int? Points { get; set; }
            public int? Xp { get; set; }
            public ICollection<Achievement> UnlockedAchievements { get; set; }

            public static QuizAttemptResults error(string message)
            {
                return new QuizAttemptResults
                {
                    Success = false,
                    Message = message
                };
            }
        }

        public static QuizAttemptResults SubmitAttempt(int userId, int quizId, IDictionary<int, int> answers)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(userId);
                if (user == null) return QuizAttemptResults.error("User not found.");

                var quiz = db.Quizzes.Find(quizId);
                if (quiz == null) return QuizAttemptResults.error("Quiz not found.");

                int score = 0;
                int attempted = 0;
                var attemptAnswers = new List<QuizAttemptAnswer>();
                foreach (var question in quiz.Questions)
                {
                    int? choiceId;
                    int selectedChoice;
                    if (answers.TryGetValue(question.Id, out selectedChoice))
                    {
                        choiceId = selectedChoice;
                    } else
                    {
                        choiceId = null;
                    }

                    var choice = question.Choices.FirstOrDefault(c => c.Id == choiceId);

                    if (choice != null)
                    {
                        attempted++;
                        if (choice.IsCorrect) score++;
                    }

                    attemptAnswers.Add(new QuizAttemptAnswer
                    {
                        QuestionId = question.Id,
                        ChoiceId = choice?.Id,
                    });
                }

                int maxScore = quiz.Questions.Count;
                int percentage = (int)Math.Round((double)score / maxScore * 100);

                // TODO MAKE ADJUSTABLE
                var passed = percentage > 70;
                int xp = LevelingService.CalculateXpForQuiz(attempted, score);
                int points = 0;
                if (passed)
                {
                    bool newlyCompleted = !user.QuizAttempts.Any(qa => qa.QuizId == quiz.Id && qa.Passed);
                    if (newlyCompleted)
                    {
                        points = passed ? PointService.CalculateForQuiz(maxScore) : 0;
                    }
                }

                user.XP += xp;
                user.Points += points;

                user.QuizAttempts.Add(new QuizAttempt
                {
                    QuizId = quiz.Id,
                    CompletedAt = DateTime.Now,
                    Answers = attemptAnswers,
                    Score = score,
                    Passed = passed,
                });


                var output = AchievementService.IncrementProgress(db, user, Achievement.TaskType.CompleteQuizzes);
                if (!output.Success) return QuizAttemptResults.error(output.Message);

                var unlockedAchievements = output.UnlockedAchievements;

                if (score == maxScore)
                {
                    var output2 = AchievementService.IncrementProgress(db, user, Achievement.TaskType.CompletePerfectQuizzes);
                    if (!output2.Success) return QuizAttemptResults.error(output2.Message);

                    unlockedAchievements = unlockedAchievements
                        .Concat(output2.UnlockedAchievements)
                        .ToList();
                }

                db.SaveChanges();

                AuthService.RefreshCurrentUser(db, user);

                return new QuizAttemptResults
                {
                    Success = true,
                    Message = "Quiz attempt submitted successfully.",
                    Passed = passed,
                    Score = score,
                    MaxScore = maxScore,
                    Percentage = percentage,
                    Points = points,
                    Xp = xp,
                    UnlockedAchievements = unlockedAchievements
                };
            }
        }
    }
}