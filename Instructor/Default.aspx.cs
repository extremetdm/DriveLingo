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

namespace DriveLingo.Instructor
{
    public partial class Dashboard : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Instructor);

            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            using (var db = new AppDbContext())
            {
                litTotalQuestionsCount.Text = db.Questions.Count().ToString();

                var attempts = db.QuizAttempts
                    .Where(a => a.CompletedAt != null)
                    .Select(a => a.Passed)
                    .ToList();

                var totalAttempts = attempts.Count;
                litTotalAttemptsCount.Text = totalAttempts.ToString();

                if (totalAttempts > 0)
                {
                    int totalPassed = attempts.Count(passed => passed);
                    int rate = (int)Math.Round((double)totalPassed / totalAttempts * 100);
                    litAveragePassRate.Text = rate + "%";
                }
                else
                {
                    litAveragePassRate.Text = "100%";
                }
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}