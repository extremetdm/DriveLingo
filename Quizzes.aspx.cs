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
                //rptQuizzes.DataSource = db.Quizzes
                //    .Include(q => q.Lesson)
                //    .Include(q => q.Questions)
                //    .Include(q => q.Attempts.Where(a => a.UserId == CurrentUser.Id && a.Passed))
                //    .ToList()
                //    .Select(q => new QuizDetails
                //    {
                //        Id = q.Id,
                //        Lesson = q.Lesson.Title,
                //        Title = q.Title,
                //        Points = 100, // todo CHANGE THIS
                //        TotalQuestions = q.Questions.Count,
                //        Passed = q.Attempts.Count > 0
                //    });

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
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var question = (Question)e.Item.DataItem;
                RadioButtonList rblOptions = (RadioButtonList)e.Item.FindControl("rblOptions");

                if (rblOptions != null && question != null)
                {
                    rblOptions.Items.Clear();
                    foreach (var choice in question.Choices)
                    {
                        rblOptions.Items.Add(new ListItem(choice.Text, choice.Id.ToString()));
                    }
                }
            }
        }

        protected void btnSubmitExam_Click(object sender, EventArgs e)
        {
            var activeQuiz = Session["ActiveQuiz"] as Quiz;

            if (activeQuiz == null || CurrentUser == null) return;

            bool isSim = Session["IsSimulationMode"] != null && (bool)Session["IsSimulationMode"];

            int correctCount = 0;
            int total = activeQuiz.Questions.Count;

            int cbScore = 0, aScore = 0, bScore = 0, cScore = 0;
            
            var answers = new List<QuizAttemptAnswer>();

            foreach (RepeaterItem item in rptQuestions.Items)
            {
                HiddenField hfQuestionId = (HiddenField)item.FindControl("hfQuestionId");
                RadioButtonList rblOptions = (RadioButtonList)item.FindControl("rblOptions");


                if (hfQuestionId != null && rblOptions != null)
                {
                    int questionId = int.Parse(hfQuestionId.Value);
                    int choiceId = int.Parse(rblOptions.SelectedValue);
                    answers.Add(new QuizAttemptAnswer
                    {
                        QuestionId = questionId,
                        ChoiceId = choiceId
                    });

                    if (activeQuiz.Questions.FirstOrDefault(q => q.Id == questionId)
                        .Choices
                        .FirstOrDefault(c => c.Id == choiceId)
                        .IsCorrect
                    )
                    {
                        correctCount++;
                    }

                    //var question = activeQuiz.Questions.FirstOrDefault(q => q.Id == hfQuestionId.Value);
                    //if (question != null && rblOptions.SelectedIndex != -1)
                    //{
                    //    int selectedIndex = Convert.ToInt32(rblOptions.SelectedValue);
                    //    if (selectedIndex == question.CorrectIndex)
                    //    {
                    //        correctCount++;
                    //        if (isSim)
                    //        {
                    //            if (question.Section == "ColorBlindness") cbScore++;
                    //            else if (question.Section == "SectionA") aScore++;
                    //            else if (question.Section == "SectionB") bScore++;
                    //            else if (question.Section == "SectionC") cScore++;
                    //        }
                    //    }
                    //}
                }
            }

            int percentage = total > 0 ? (int)Math.Round((double)correctCount / total * 100) : 0;
            bool passed = false;

            int awardedPoints = 0;

            int awardedXP = isSim ? (correctCount * 10) : (correctCount * 50); //TODO CHANGE THIS

            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(CurrentUser.Id);

                if (isSim)
                {
                    //// Sectional pass criteria:
                    //// Color Blindness: 8/8 (100%)
                    //// Section A: 17/21
                    //// Section B: 28/35
                    //// Section C: 11/14
                    //// Overall score: >= 80% (>= 63/78)
                    //bool cbPassed = (cbScore == 8);
                    //bool aPassed = (aScore >= 17);
                    //bool bPassed = (bScore >= 28);
                    //bool cPassed = (cScore >= 11);

                    //passed = (percentage >= 80) && cbPassed && aPassed && bPassed && cPassed;
                    //awardedPoints = 0; // No points for simulation

                    //// Populate Simulation Breakdown UI
                    //pnlSimBreakdown.Visible = true;
                    //litSimCbScore.Text = cbScore + " / 8";
                    //lblSimCbStatus.Text = cbPassed ? "PASS 🟢" : "FAIL 🔴";
                    //lblSimCbStatus.CssClass = cbPassed ? "badge badge-success" : "badge badge-danger";

                    //litSimSecAScore.Text = aScore + " / 21";
                    //lblSimSecAStatus.Text = aPassed ? "PASS 🟢" : "FAIL 🔴";
                    //lblSimSecAStatus.CssClass = aPassed ? "badge badge-success" : "badge badge-danger";

                    //litSimSecBScore.Text = bScore + " / 35";
                    //lblSimSecBStatus.Text = bPassed ? "PASS 🟢" : "FAIL 🔴";
                    //lblSimSecBStatus.CssClass = bPassed ? "badge badge-success" : "badge badge-danger";

                    //litSimSecCScore.Text = cScore + " / 14";
                    //lblSimSecCStatus.Text = cPassed ? "PASS 🟢" : "FAIL 🔴";
                    //lblSimSecCStatus.CssClass = cPassed ? "badge badge-success" : "badge badge-danger";
                }
                else
                {
                    pnlSimBreakdown.Visible = false;
                    passed = percentage >= 70;

                    if (passed)
                    {
                        bool newlyCompleted = !user.QuizAttempts.Any(qa => qa.QuizId == activeQuiz.Id && qa.Passed);
                        if (newlyCompleted)
                        {
                            awardedPoints = 100; //TODO CHANGE THIS
                        }
                    }
                }

                user.Points += awardedPoints;
                user.XP += awardedXP;
                user.QuizAttempts.Add(new QuizAttempt
                {
                    QuizId = activeQuiz.Id,
                    CompletedAt = DateTime.Now,
                    Answers = answers,
                    Score = correctCount,
                    Passed = passed,
                });

                db.SaveChanges();
            }

            //var attempt = new QuizAttempt
            //{
            //    Id = "att_" + Guid.NewGuid().ToString("N").Substring(0, 8),
            //    UserId = CurrentUser.Id.ToString(),
            //    QuizId = activeQuiz.Id,
            //    QuizTitle = isSim ? "Official JPJ Exam Simulation (78 Qs)" : activeQuiz.Title,
            //    Score = correctCount,
            //    TotalQuestions = total,
            //    Percentage = percentage,
            //    Passed = passed,
            //    DateTaken = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            //    IsSimulation = isSim,
            //    ColorBlindScore = cbScore,
            //    SectionAScore = aScore,
            //    SectionBScore = bScore,
            //    SectionCScore = cScore
            //};

            //var repo = AppStateRepository.GetCurrent();
            //repo.Attempts.Add(attempt);

            litResultIcon.Text = passed ? "🎉" : "⚠️";
            litResultHeader.Text = isSim ? (passed ? "Official JPJ Simulation PASSED!" : "Official JPJ Simulation FAILED") : (passed ? "Exam Passed!" : "Exam Needs Improvement");
            litResultScore.Text = correctCount + " / " + total;
            litResultPercentage.Text = percentage.ToString();
            litAwardedPoints.Text = awardedPoints.ToString();
            litAwardedXP.Text = awardedXP.ToString();

            if (isSim)
            {
                if (passed)
                    ShowNotification("Congratulations! You PASSED the Official JPJ 78-Question Simulation Exam (+ " + awardedXP + " XP)");
                else
                    ShowNotification("Simulation Exam Result: FAILED. Please review the sectional criteria failure reasons below.");
            }
            else if (awardedPoints > 0)
            {
                ShowNotification("Quiz Passed! You earned +" + awardedPoints + " Points & +" + awardedXP + " XP!");
            }
            else if (passed)
            {
                ShowNotification("Quiz Passed! +" + awardedXP + " XP earned. (Points bonus was previously claimed for this quiz)");
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