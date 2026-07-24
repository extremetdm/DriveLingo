using DriveLingo.Database;
using DriveLingo.Database.Models;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo
{
    public partial class Dashboard : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            if (!IsPostBack)
            {
                BindDashboardData(CurrentUser);
            }
        }

        struct QuizAttemptSummary
        {
            public string QuizTitle { get; set; }
            public double Percentage { get; set; }
            public int Score { get; set; }
            public bool Passed { get; set; }
            public DateTime CompletedAt { get; set; }

        }

        private void BindDashboardData(User user)
        {
            using (var db = new AppDbContext())
            {
                var attempts = db.QuizAttempts.Where(a => a.UserId == user.Id)
                    .Where(a => a.CompletedAt != null)
                    .Include(a => a.Quiz)
                    .Include(a => a.Answers)
                    .ToList()
                    .Select(a => new QuizAttemptSummary
                    {
                        QuizTitle = a.Quiz.Title,
                        Score = a.Score,
                        Percentage = Math.Round(100.0 * (double)a.Score / a.Answers.Count, 1),
                        Passed = a.Passed,
                        CompletedAt = a.CompletedAt
                    })
                    .ToList();

                if (attempts.Count > 0)
                {
                    int passedCount = attempts.Count(a => a.Passed);
                    int rate = (int)Math.Round((double)passedCount / attempts.Count * 100);
                    litPassRate.Text = rate + "%";
                }
                else
                {
                    litPassRate.Text = "0%";
                }

                gvAttempts.DataSource = attempts;
                gvAttempts.DataBind();
            }

            litLevel.Text = user.CurrentLevel.ToString();
            litPoints.Text = user.Points.ToString();
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
            // todo add error msg notification
        }
    }
}