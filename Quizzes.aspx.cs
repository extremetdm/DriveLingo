using DriveLingo.Data;
using DriveLingo.Database;
using DriveLingo.Database.Models;
using DriveLingo.Services;
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
    public partial class Quizzes : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();

            if (!IsPostBack)
            {
                BindQuizzes();
            }
        }

        // --- Exam Simulator Handlers ---
        struct QuizDetails
        {
            public int Id { get; set; }
            public string Lesson { get; set; }
            public string Title { get; set; }
            public int Points { get; set; }
            public int TotalQuestions { get; set; }
            public bool Passed { get; set; }

        }

        private void BindQuizzes()
        {
            using (var db = new AppDbContext())
            {

                rptQuizzes.DataSource = db.Quizzes
                    .Select(q => new
                    {
                        q.Id,
                        LessonTitle = q.Lesson.Title,
                        q.Title,
                        QuestionCount = q.Questions.Count,
                        HasPassed = q.Attempts.Any(a => a.UserId == CurrentUser.Id && a.Passed)
                    })
                    .ToList()
                    .Select(q => new QuizDetails
                    {
                        Id = q.Id,
                        Lesson = q.LessonTitle,
                        Title = q.Title,
                        Points = 100, // todo CHANGE THIS
                        TotalQuestions = q.QuestionCount,
                        Passed = q.HasPassed
                    })
                    .ToList();

                rptQuizzes.DataBind();
            }
        }

        protected void rptQuizzes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var quiz = (QuizDetails) e.Item.DataItem;
                PlaceHolder phQuizCompletedBadge = (PlaceHolder)e.Item.FindControl("phQuizCompletedBadge");

                if (phQuizCompletedBadge != null)
                {
                    if (quiz.Passed)
                    {
                        phQuizCompletedBadge.Visible = true;
                    }
                }
            }
        }

        protected void rptQuizzes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "StartQuiz")
            {
                var quizId = int.Parse(e.CommandArgument.ToString());

                using (var db = new AppDbContext())
                {
                    var quiz = db.Quizzes
                        .Include(quizz => quizz.Questions.Select(q => q.Choices))
                        .FirstOrDefault(q => q.Id == quizId);

                    if (quiz != null)
                    {
                        Session["ActiveQuiz"] = quiz;
                        Session["IsSimulationMode"] = false;
                        litExamTitle.Text = quiz.Title;
                        rptQuestions.DataSource = quiz.Questions;
                        rptQuestions.DataBind();

                        pnlQuizList.Visible = false;
                        pnlActiveExam.Visible = true;
                        pnlExamResult.Visible = false;
                    }

                }
            }
        }

        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item
                && e.Item.ItemType != ListItemType.AlternatingItem
            ) return;

            var question = (Question)e.Item.DataItem;
            var rblOptions = (RadioButtonList)e.Item.FindControl("rblOptions");

            rblOptions.DataSource = question.Choices.ToList();
            rblOptions.DataTextField = "Text";
            rblOptions.DataValueField = "Id";
            rblOptions.DataBind();
        }

        protected void btnSubmitExam_Click(object sender, EventArgs e)
        {
            var quiz = Session["ActiveQuiz"] as Quiz;

            bool isSim = Session["IsSimulationMode"] != null && (bool)Session["IsSimulationMode"];

            var answers = new Dictionary<int, int>();

            foreach (RepeaterItem item in rptQuestions.Items)
            {
                HiddenField hfQuestionId = (HiddenField)item.FindControl("hfQuestionId");
                RadioButtonList rblOptions = (RadioButtonList)item.FindControl("rblOptions");

                int questionId, choiceId;

                if (!int.TryParse(hfQuestionId.Value, out questionId))
                {
                    ShowNotification("Invalid question.");
                    return;
                }

                if (!int.TryParse(rblOptions.SelectedValue, out choiceId))
                {
                    ShowNotification("Invalid choice.");
                    return;
                }

                answers.Add(questionId, choiceId);
            }

            var output = QuizAttemptService.SubmitAttempt(CurrentUser.Id, quiz.Id, answers);
            if (!output.Success)
            {
                ShowNotification(output.Message);
            }

            litResultIcon.Text = output.Passed ?? false ? "🎉" : "⚠️";
            litResultHeader.Text = output.Passed ?? false ? "Exam Passed!" : "Exam Needs Improvement";
                //(passed ? "Official JPJ Simulation PASSED!" : "Official JPJ Simulation FAILED") : 
            litResultScore.Text = output.Score + " / " + output.MaxScore;
            litResultPercentage.Text = output.Percentage.ToString();
            litAwardedPoints.Text = output.Points.ToString();
            litAwardedXP.Text = output.Xp.ToString();

            //if (isSim)
            //{
            //    if (passed)
            //        ShowNotification("Congratulations! You PASSED the Official JPJ 78-Question Simulation Exam (+ " + awardedXP + " XP)");
            //    else
            //        ShowNotification("Simulation Exam Result: FAILED. Please review the sectional criteria failure reasons below.");
            //}
            if (output.Points > 0)
            {
                ShowNotification("Quiz Passed! You earned +" + output.Points + " Points & +" + output.Xp + " XP!");
            }
            else if (output.Passed ?? false)    
            {
                ShowNotification("Quiz Passed! +" + output.Xp + " XP earned. (Points bonus was previously claimed for this quiz)");
            }

            Session["IsSimulationMode"] = false;
            pnlActiveExam.Visible = false;
            pnlExamResult.Visible = true;

            BindQuizzes();
            ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
        }

        protected void btnCancelExam_Click(object sender, EventArgs e)
        {
            pnlQuizList.Visible = true;
            pnlActiveExam.Visible = false;
            pnlExamResult.Visible = false;
        }

        protected void btnBackToQuizzes_Click(object sender, EventArgs e)
        {
            pnlQuizList.Visible = true;
            pnlActiveExam.Visible = false;
            pnlExamResult.Visible = false;
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
            // todo add error msg notification
        }
    }
}