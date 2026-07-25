using DriveLingo.Data;
using DriveLingo.Models;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo
{
    public partial class Achievements : AuthPage
    {
        public class AchievementProgressViewModel
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }
            public int XpBonus { get; set; }
            public int TargetCount { get; set; }
            public int CurrentProgress { get; set; }
            public int ProgressPercentage { get; set; }
            public bool IsUnlocked { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();

            if (!IsPostBack)
            {
                BindAchievements();
            }
        }

        private void BindAchievements()
        {
            var state = AppStateRepository.GetCurrent();
            var user = sessionUser ?? state.Users.FirstOrDefault(u => u.Role == "learner") ?? new User();

            // Compute metrics for current user
            int userQuizAttemptsCount = state.Attempts.Count(a => a.UserId == user.Id || a.UserId == "usr_learner");
            int userPerfectScoreCount = state.Attempts.Count(a => (a.UserId == user.Id || a.UserId == "usr_learner") && a.Percentage >= 100);
            int userReadMaterialsCount = user.ReadMaterials != null ? user.ReadMaterials.Count : 0;

            var list = new List<AchievementProgressViewModel>();

            foreach (var ach in state.Achievements)
            {
                int current = 0;

                if (ach.MetricType == "quiz_count")
                {
                    current = userQuizAttemptsCount;
                }
                else if (ach.MetricType == "perfect_score")
                {
                    current = userPerfectScoreCount;
                }
                else if (ach.MetricType == "materials_read")
                {
                    current = userReadMaterialsCount;
                }
                else
                {
                    current = userQuizAttemptsCount;
                }

                int target = ach.TargetCount > 0 ? ach.TargetCount : 1;
                bool unlocked = current >= target || (user.Achievements != null && user.Achievements.Contains(ach.Id));
                if (unlocked) current = target;

                int percentage = (int)Math.Round((double)current / target * 100);
                if (percentage > 100) percentage = 100;

                list.Add(new AchievementProgressViewModel
                {
                    Id = ach.Id,
                    Title = ach.Title,
                    Description = ach.Description,
                    Icon = ach.Icon,
                    XpBonus = ach.XpBonus,
                    TargetCount = target,
                    CurrentProgress = current,
                    ProgressPercentage = percentage,
                    IsUnlocked = unlocked
                });
            }

            rptAchievements.DataSource = list;
            rptAchievements.DataBind();
        }

        private User sessionUser
        {
            get => Session["CurrentUser"] as User;
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}