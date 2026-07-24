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
            //User user = Session["CurrentUser"] as User;
            //if (user == null || (user.Role != "educator" && user.Role != "admin"))
            //{
            //    Response.Redirect("~/Login.aspx");
            //    return;
            //}

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
            gvLearnerReports.DataSource = repo.Attempts.AsEnumerable().Reverse().ToList();
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
            string imageUrl = txtQuestionImageUrl.Text.Trim();

            if (fileQuestionImage.HasFile)
            {
                try
                {
                    string uploadsDir = Server.MapPath("~/uploads/");
                    if (!System.IO.Directory.Exists(uploadsDir))
                    {
                        System.IO.Directory.CreateDirectory(uploadsDir);
                    }

                    string fileName = "sign_" + Guid.NewGuid().ToString("N").Substring(0, 8) + System.IO.Path.GetExtension(fileQuestionImage.FileName);
                    fileQuestionImage.SaveAs(uploadsDir + fileName);
                    imageUrl = "uploads/" + fileName;
                }
                catch (Exception ex)
                {
                    ShowNotification("Image upload error: " + ex.Message);
                }
            }

            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(opt1) || string.IsNullOrEmpty(opt2))
            {
                ShowNotification("Please provide question text and options.");
                return;
            }

            var repo = AppStateRepository.GetCurrent();
            string editingId = hfEditingQuestionId.Value;

            if (!string.IsNullOrEmpty(editingId))
            {
                // EDIT EXISTING QUESTION
                Question existingQ = null;
                foreach (var quiz in repo.Quizzes)
                {
                    existingQ = quiz.Questions.FirstOrDefault(q => q.Id == editingId);
                    if (existingQ != null) break;
                }

                if (existingQ != null)
                {
                    existingQ.QuizId = quizId;
                    existingQ.Text = text;
                    existingQ.Options = new List<string> { opt1, opt2, opt3, opt4 };
                    existingQ.CorrectIndex = correctIndex;
                    existingQ.Explanation = explanation;
                    if (!string.IsNullOrEmpty(imageUrl)) existingQ.ImageUrl = imageUrl;

                    ShowNotification("Question details for " + existingQ.Id + " saved successfully!");
                    ResetQuestionForm();
                    BindQuestionBank();
                    BindMetrics();
                }
            }
            else
            {
                // CREATE NEW QUESTION
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
                        Explanation = explanation,
                        ImageUrl = imageUrl
                    };

                    targetQuiz.Questions.Add(newQuestion);
                    ShowNotification("New question successfully saved to " + targetQuiz.Title + "!");

                    ResetQuestionForm();
                    BindQuestionBank();
                    BindMetrics();
                }
            }
        }

        protected void btnCancelQuestionEdit_Click(object sender, EventArgs e)
        {
            ResetQuestionForm();
            ShowNotification("Edit cancelled.");
        }

        private void ResetQuestionForm()
        {
            hfEditingQuestionId.Value = "";
            litFormTitle.Text = "➕ Create New Practice Question";
            btnAddQuestion.Text = "➕ Save Question to Database";
            btnCancelQuestionEdit.Visible = false;

            txtQuestionText.Text = "";
            txtQuestionImageUrl.Text = "";
            txtOpt1.Text = "";
            txtOpt2.Text = "";
            txtOpt3.Text = "";
            txtOpt4.Text = "";
            txtExplanation.Text = "";
        }

        protected void gvQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditQuestion")
            {
                string questionId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();

                Question targetQ = null;
                foreach (var quiz in repo.Quizzes)
                {
                    targetQ = quiz.Questions.FirstOrDefault(q => q.Id == questionId);
                    if (targetQ != null) break;
                }

                if (targetQ != null)
                {
                    hfEditingQuestionId.Value = targetQ.Id;
                    if (ddlQuizTarget.Items.FindByValue(targetQ.QuizId) != null)
                    {
                        ddlQuizTarget.SelectedValue = targetQ.QuizId;
                    }
                    txtQuestionText.Text = targetQ.Text;
                    txtQuestionImageUrl.Text = targetQ.ImageUrl;

                    txtOpt1.Text = targetQ.Options.Count > 0 ? targetQ.Options[0] : "";
                    txtOpt2.Text = targetQ.Options.Count > 1 ? targetQ.Options[1] : "";
                    txtOpt3.Text = targetQ.Options.Count > 2 ? targetQ.Options[2] : "";
                    txtOpt4.Text = targetQ.Options.Count > 3 ? targetQ.Options[3] : "";

                    if (ddlCorrectIndex.Items.FindByValue(targetQ.CorrectIndex.ToString()) != null)
                    {
                        ddlCorrectIndex.SelectedValue = targetQ.CorrectIndex.ToString();
                    }
                    txtExplanation.Text = targetQ.Explanation;

                    litFormTitle.Text = "✏️ Edit Practice Question (" + targetQ.Id + ")";
                    btnAddQuestion.Text = "💾 Save Question Changes";
                    btnCancelQuestionEdit.Visible = true;

                    ShowNotification("Question " + targetQ.Id + " loaded into editor below. Make your changes and click 'Save Question Changes'.");
                }
            }
            else if (e.CommandName == "Delete")
            {
                string questionId = e.CommandArgument.ToString();
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
        }

        protected void gvQuestions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Handled in gvQuestions_RowCommand
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

        protected void rptForumModeration_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DiscussionThread thread = e.Item.DataItem as DiscussionThread;
                Repeater rptReplies = e.Item.FindControl("rptEducatorReplies") as Repeater;

                if (thread != null && rptReplies != null)
                {
                    rptReplies.DataSource = thread.Replies;
                    rptReplies.DataBind();
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
