using DriveLingo.Database;
using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace DriveLingo.Services
{
    public static class AchievementService
    {
        public static ServiceStatusOutput IncrementProgress(AppDbContext db, User user, Achievement.TaskType task)
        {
            var progress = user.AchievementProgress.FirstOrDefault(p => p.Task == task);
            if (progress == null)
            {
                progress = new AchievementProgress
                {
                    Task = task,
                };
                user.AchievementProgress.Add(progress);
            }

            progress.Progress += 1;

            var completedAchievementIds = user.Achievements.Select(a => a.AchievementId);

            var newAchievements = db.Achievements
                .Where(a => a.Task == task)
                .Where(a => a.Target <= progress.Progress)
                .Where(a => !completedAchievementIds.Contains(a.Id))
                .ToList();

            foreach (var achievement in newAchievements)
            {
                user.Achievements.Add(new CompletedAchievement
                {
                    AchievementId = achievement.Id,
                });
                user.XP += achievement.Xp;
                user.Points += achievement.Points;
            }

            AuthService.RefreshCurrentUser(user);

            return ServiceStatusOutput.success("Successfully updated achievements.", newAchievements);
        }

        public struct AchievementStatus
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }
            public int Progress { get; set; }
            public int Target { get; set; }
            public DateTime? CompletedAt { get; set; }
            public int Xp { get; set; }
            public int Points { get; set; }
        }

        public struct AchievementFetchOutput
        {
            public bool Success { get; }
            public string Message { get; }
            public ICollection<AchievementStatus> Achievements { get; }

            private AchievementFetchOutput(bool success, string message, ICollection<AchievementStatus> achievements)
            {
                Success = success;
                Message = message;
                Achievements = achievements;
            }
            public static AchievementFetchOutput error(string message)
            {
                return new AchievementFetchOutput(false, message, null);
            }

            public static AchievementFetchOutput success(string message, ICollection<AchievementStatus> achievements)
            {
                return new AchievementFetchOutput(true, message, achievements);
            }
        }

        public static AchievementFetchOutput GetUserAchievements(int userId)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(userId);
                if (user == null) return AchievementFetchOutput.error("User not found.");

                var progresses = user.AchievementProgress
                    .ToDictionary(ap => ap.Task, ap => ap.Progress);

                var completed = user.Achievements
                    .ToDictionary(ca => ca.AchievementId, ca => ca.CreatedAt);

                var achievements = db.Achievements.ToList()
                    .Select(a =>
                    {
                        int progress;
                        if (!progresses.TryGetValue(a.Task, out progress))
                        {
                            progress = 0;
                        }

                        DateTime? completedAt;
                        DateTime createdAt;
                        if (completed.TryGetValue(a.Id, out createdAt)) {
                            completedAt = createdAt;
                        } else
                        {
                            completedAt = null;
                        }

                        return new AchievementStatus
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                            Icon = a.Icon,
                            Progress = Math.Min(progress, a.Target),
                            Target = a.Target,
                            CompletedAt = completedAt,
                            Xp = a.Xp,
                            Points = a.Points,
                        };
                    })
                    .ToList();

                return AchievementFetchOutput.success("Fetched all achievements.", achievements);
            }
        }
    }
}