using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DriveLingo.Data;
using DriveLingo.Models;

namespace DriveLingo
{
    public partial class Educator : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User user = Session["CurrentUser"] as User;
            if (user == null || (user.Role != "educator" && user.Role != "admin"))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindQuizDropdown();
                BindQuestionBank();
                BindMetrics();
                BindForumForModeration();
                BindLearnerReports();

                string tab = Request.QueryString["tab"];
                SwitchTab(tab);
            }
        }

        private void SwitchTab(string tab)
        {
            pnlDashboard.Visible = (tab == "dashboard" || string.IsNullOrEmpty(tab));
            pnlQuizzes.Visible = (tab == "quizzes");
            pnlForum.Visible = (tab == "forum");
            pnlReports.Visible = (tab == "reports");
        }

        private void BindQuizDropdown()
        {
            var repo = AppStateRepository.GetCurrent();
            ddlQuizTarget.DataSource = repo.Quizzes;
            ddlQuizTarget.DataTextField = "Title";
            ddlQuizTarget.DataValueField = "Id";
            ddlQuizTarget.DataBind();
        }

        private void BindQuestionBank()
        {
            var repo = AppStateRepository.GetCurrent();
            var allQuestions = new List<Question>();
            foreach (var quiz in repo.Quizzes)
            {
                allQuestions.AddRange(quiz.Questions);
            }

            gvQuestions.DataSource = allQuestions;
            gvQuestions.DataBind();

            litTotalQuestionsCount.Text = allQuestions.Count.ToString();
        }

        private void BindMetrics()
        {
            var repo = AppStateRepository.GetCurrent();
            litTotalAttemptsCount.Text = repo.Attempts.Count.ToString();

            if (repo.Attempts.Count > 0)
            {
                int passed = repo.Attempts.Count(a => a.Passed);
                int rate = (int)Math.Round((double)passed / repo.Attempts.Count * 100);
                litAveragePassRate.Text = rate + "%";
            }
            else
            {
                litAveragePassRate.Text = "100%";
            }
        }

        private void BindForumForModeration()
        {
            var repo = AppStateRepository.GetCurrent();
            rptForumModeration.DataSource = repo.Discussions.OrderByDescending(d => d.DatePosted).ToList();
            rptForumModeration.DataBind();
        }

        private void BindLearnerReports()
        {
            var repo = AppStateRepository.GetCurrent();
            gvLearnerReports.DataSource = repo.Attempts.OrderByDescending(a => a.DateTaken).ToList();
            gvLearnerReports.DataBind();
        }

        protected void btnAddQuestion_Click(object sender, EventArgs e)
        {
            string quizId = ddlQuizTarget.SelectedValue;
            string text = txtQuestionText.Text.Trim();
            string opt1 = txtOpt1.Text.Trim();
            string opt2 = txtOpt2.Text.Trim();
            string opt3 = txtOpt3.Text.Trim();
            string opt4 = txtOpt4.Text.Trim();
            int correctIndex = Convert.ToInt32(ddlCorrectIndex.SelectedValue);
            string explanation = txtExplanation.Text.Trim();

            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(opt1) || string.IsNullOrEmpty(opt2))
            {
                ShowNotification("Please provide question text and options.");
                return;
            }

            var repo = AppStateRepository.GetCurrent();
            var targetQuiz = repo.Quizzes.FirstOrDefault(q => q.Id == quizId);

            if (targetQuiz != null)
            {
                var newQuestion = new Question
                {
                    Id = "q_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                    QuizId = targetQuiz.Id,
                    Text = text,
                    Options = new List<string> { opt1, opt2, opt3, opt4 },
                    CorrectIndex = correctIndex,
                    Explanation = explanation
                };

                targetQuiz.Questions.Add(newQuestion);
                ShowNotification("New question successfully saved to " + targetQuiz.Title + "!");

                txtQuestionText.Text = "";
                txtOpt1.Text = "";
                txtOpt2.Text = "";
                txtOpt3.Text = "";
                txtOpt4.Text = "";
                txtExplanation.Text = "";

                BindQuestionBank();
                BindMetrics();
            }
        }

        protected void gvQuestions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string questionId = gvQuestions.DataKeys[e.RowIndex].Value.ToString();
            var repo = AppStateRepository.GetCurrent();

            foreach (var quiz in repo.Quizzes)
            {
                var q = quiz.Questions.FirstOrDefault(x => x.Id == questionId);
                if (q != null)
                {
                    quiz.Questions.Remove(q);
                    ShowNotification("Question deleted from database.");
                    break;
                }
            }

            BindQuestionBank();
            BindMetrics();
        }

        protected void rptForumModeration_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Reply")
            {
                string threadId = e.CommandArgument.ToString();
                TextBox txtReply = (TextBox)e.Item.FindControl("txtEducatorReply");
                User currentUser = Session["CurrentUser"] as User;

                if (txtReply != null && !string.IsNullOrEmpty(txtReply.Text.Trim()) && currentUser != null)
                {
                    var repo = AppStateRepository.GetCurrent();
                    var thread = repo.Discussions.FirstOrDefault(d => d.Id == threadId);

                    if (thread != null)
                    {
                        thread.Replies.Add(new DiscussionReply
                        {
                            Id = "rep_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                            AuthorId = currentUser.Id,
                            AuthorName = currentUser.Name,
                            AuthorRole = currentUser.Role,
                            AuthorAvatar = currentUser.Avatar,
                            Content = txtReply.Text.Trim(),
                            DatePosted = DateTime.Now.ToString("yyyy-MM-dd"),
                            IsEducatorAnswer = true
                        });

                        ShowNotification("Instructor verified response posted!");
                        txtReply.Text = "";
                        BindForumForModeration();
                    }
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
