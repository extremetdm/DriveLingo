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
    public partial class LearnerPerformance : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Instructor);

            if (!IsPostBack)
            {
                BindLearnerReports();
            }
        }

        struct LearnerPerformanceData
        {
            public string Username { get; set; }
            public string QuizTitle { get; set; }
            public int Score { get; set; }
            public int Percentage { get; set; }
            public bool Passed { get; set; }
            public DateTime CompletedAt { get; set; }
        }

        private void BindLearnerReports()
        {
            using (var db = new AppDbContext())
            {
                gvLearnerReports.DataSource = db.QuizAttempts.Where(a => a.CompletedAt != null)
                    .Select(a => new
                    {
                        a.User.Username,
                        QuizTitle = a.Quiz.Title,
                        a.Score,
                        Percentage = Math.Round((double)a.Score / a.Quiz.Questions.Count * 100),
                        a.Passed,
                        a.CompletedAt
                    })
                    .ToList()
                    .Select(a => new LearnerPerformanceData
                    {
                        Username = a.Username,
                        QuizTitle = a.QuizTitle,
                        Score = a.Score,
                        Percentage = (int) a.Percentage,
                        Passed = a.Passed,
                        CompletedAt = a.CompletedAt
                    });
                gvLearnerReports.DataBind();
            }
        }
        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}