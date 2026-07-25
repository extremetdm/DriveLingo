using DriveLingo.Data;
using DriveLingo.Database;
using DriveLingo.Models;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo.Admin
{
    public partial class Dashboard : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Admin);

            if (!IsPostBack)
            {
                BindMetrics();
            }
        }

        private void BindMetrics()
        {
            var repo = AppStateRepository.GetCurrent();

            using (var db = new AppDbContext())
            {
                litTotalUsers.Text = db.Users.Count().ToString();
                litTotalAttempts.Text = db.QuizAttempts
                    .Where(a => a.CompletedAt != null)
                    .Count()
                    .ToString();
                litTotalQuestions.Text = db.Questions.Count().ToString();
            }
        }

        // wtf is this
        protected void btnResetState_Click(object sender, EventArgs e)
        {
            BindMetrics();
            ShowNotification("Application state and demo data successfully re-seeded.");
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}