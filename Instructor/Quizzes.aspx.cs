using DriveLingo.Data;
using DriveLingo.Database;
using DriveLingo.Database.Models;
using DriveLingo.UI;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo.Instructor
{
    public partial class Quizzes : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Instructor);

            if (!IsPostBack)
            {
                ResetQuestionForm();
                BindData();
            }
        }

        private void BindData()
        {
            using (var db = new AppDbContext())
            {
                var quizzes = db.Quizzes.Include(quiz => quiz.Questions.Select(q => q.Choices))
                    .ToList();

                ddlQuizTarget.DataSource = quizzes;
                ddlQuizTarget.DataTextField = "Title";
                ddlQuizTarget.DataValueField = "Id";
                ddlQuizTarget.DataBind();

                gvQuestions.DataSource = db.Questions
                    .Include(q => q.Quiz)
                    .Include(q => q.Choices)
                    .ToList();
                gvQuestions.DataBind();
            }
        }

        protected void btnAddQuestion_Click(object sender, EventArgs e)
        {
            int quizId = Convert.ToInt32(ddlQuizTarget.SelectedValue);
            string text = txtQuestionText.Text.Trim();
            //string opt1 = txtOpt1.Text.Trim();
            //string opt2 = txtOpt2.Text.Trim();
            //string opt3 = txtOpt3.Text.Trim();
            //string opt4 = txtOpt4.Text.Trim();
            int correctIndex = Convert.ToInt32(ddlCorrectIndex.SelectedValue);
            string explanation = txtExplanation.Text.Trim();
            string imageUrl = txtQuestionImageUrl.Text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                ShowNotification("Please provide question text.");
                return;
            }

            int choiceFilledCount = 0;

            for (int i = 0; i < rptChoices.Items.Count; i++)
            {
                var item = rptChoices.Items[i];
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var txtChoiceText = (TextBox)item.FindControl("txtChoiceText");
                    string choiceText = txtChoiceText.Text.Trim();

                    if (!string.IsNullOrEmpty(choiceText)) choiceFilledCount++;
                    else if (i == correctIndex)
                    {
                        ShowNotification("Correct option cannot be empty.");
                        return;
                    }


                }
            }

            if (choiceFilledCount < 2) {
                ShowNotification("Please provide question options.");
                return;
            }

            if (fileQuestionImage.HasFile && fileQuestionImage.FileContent.Length > 0)
            {
                try
                {
                    string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".svg" };

                    string originalFileName = Path.GetFileName(fileQuestionImage.FileName); // Strip client path info
                    string extension = Path.GetExtension(originalFileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(extension))
                    {
                        throw new InvalidOperationException("Invalid file type uploaded.");
                    }

                    string uploadsDir = Server.MapPath("~/uploads/");
                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }

                    string safeFileName = "sign_" + Guid.NewGuid().ToString("N").Substring(0, 8) + extension;

                    string fullPath = Path.GetFullPath(Path.Combine(uploadsDir, safeFileName));

                    string canonicalUploadsDir = Path.GetFullPath(uploadsDir);
                    if (!canonicalUploadsDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    {
                        canonicalUploadsDir += Path.DirectorySeparatorChar;
                    }

                    if (!fullPath.StartsWith(canonicalUploadsDir, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new System.Security.SecurityException("Path traversal attempt detected!");
                    }

                    // 7. Save file safely
                    fileQuestionImage.SaveAs(fullPath);
                    imageUrl = "uploads/" + safeFileName;
                }
                catch (Exception ex)
                {
                    ShowNotification("Image upload error: " + ex.Message);
                }
            }


            using (var db = new AppDbContext())
            {
                Question question = null;
                string editingId = hfEditingQuestionId.Value;
                if (!string.IsNullOrEmpty(editingId))
                {
                    question = db.Questions.Find(Convert.ToInt32(editingId));
                }

                var quiz = db.Quizzes.Find(quizId);
                if (quiz == null) return;

                bool isEdit = question != null;

                if (isEdit)
                {
                    question.QuizId = quiz.Id;
                    question.Text = text;
                    if (!string.IsNullOrEmpty(imageUrl)) question.Image = imageUrl;
                } else
                {
                    question = new Question
                    {
                        Text = text
                    };
                    if (!string.IsNullOrEmpty(imageUrl)) question.Image = imageUrl;
                    quiz.Questions.Add(question);
                }

                foreach (RepeaterItem item in rptChoices.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        var hfChoiceId = (HiddenField)item.FindControl("hfChoiceId");
                        var txtChoiceText = (TextBox)item.FindControl("txtChoiceText");

                        string choiceText = txtChoiceText.Text.Trim();

                        QuestionChoice choice = null;

                        if (!string.IsNullOrEmpty(hfChoiceId.Value))
                        {
                            int choiceId = Convert.ToInt32(hfChoiceId.Value);
                            choice = question.Choices.FirstOrDefault(c => c.Id == choiceId);
                        }

                        var choiceExists = choice != null;
                        var textIsEmpty = string.IsNullOrEmpty(choiceText);

                        if (textIsEmpty)
                        {
                            if (choiceExists) db.QuestionChoices.Remove(choice);
                            continue;
                        }

                        if (!choiceExists)
                        {
                            choice = new QuestionChoice();
                            question.Choices.Add(choice);
                        }
                        choice.Text = choiceText;
                    }
                }

                db.SaveChanges();

                if (isEdit)
                {
                    ShowNotification("Question details for " + question.Id + " saved successfully!");
                }
                else
                {
                    ShowNotification("New question successfully saved to " + quiz.Title + "!");

                }

                ResetQuestionForm();
                BindData();
            }
        }

        protected void btnCancelQuestionEdit_Click(object sender, EventArgs e)
        {
            ResetQuestionForm();
            ShowNotification("Edit cancelled.");
        }

        struct Choice
        {
            public int? Id { get; set; }
            public string Text { get; set; }

        }
        private void ResetQuestionForm()
        {
            hfEditingQuestionId.Value = "";
            litFormTitle.Text = "➕ Create New Practice Question";
            btnAddQuestion.Text = "➕ Save Question to Database";
            btnCancelQuestionEdit.Visible = false;

            txtQuestionText.Text = "";
            txtQuestionImageUrl.Text = "";
            //txtOpt1.Text = "";
            //txtOpt2.Text = "";
            //txtOpt3.Text = "";
            //txtOpt4.Text = "";
            txtExplanation.Text = "";

            setChoices(new List<Choice>());
        }

        private void setChoices(ICollection<Choice> choices)
        {
            while (choices.Count < 4)
            {
                choices.Add(new Choice());
            }

            rptChoices.DataSource = choices;
            rptChoices.DataBind();

            ddlCorrectIndex.DataSource = choices.Select((c, i) => new
            {
                Label = $"Option {i + 1} ({(char)('A' + i)})",
                Value = i
            }).ToList();
            ddlCorrectIndex.DataTextField = "Label";
            ddlCorrectIndex.DataValueField = "Value";
            ddlCorrectIndex.DataBind();
        }

        protected void gvQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditQuestion")
            {
                handleEditQuestion(sender, e);
            }
            else if (e.CommandName == "DeleteQuestion")
            {
                handleDeleteQuestion(sender, e);
            }
        }

        private void handleEditQuestion(object sender, GridViewCommandEventArgs e)
        {
            int questionId = Convert.ToInt32(e.CommandArgument.ToString());

            using (var db = new AppDbContext())
            {
                var question = db.Questions.Find(questionId);
                if (question == null) return;

                hfEditingQuestionId.Value = question.Id.ToString();
                if (ddlQuizTarget.Items.FindByValue(question.QuizId.ToString()) != null)
                {
                    ddlQuizTarget.SelectedValue = question.QuizId.ToString();
                }

                txtQuestionText.Text = question.Text;
                txtQuestionImageUrl.Text = question.Image;

                var choices = question.Choices.ToList();

                setChoices(choices.Select(c => new Choice
                {
                    Id = c.Id,
                    Text = c.Text
                }).ToList());

                //txtOpt1.Text = targetQ.Options.Count > 0 ? targetQ.Options[0] : "";
                //txtOpt2.Text = targetQ.Options.Count > 1 ? targetQ.Options[1] : "";
                //txtOpt3.Text = targetQ.Options.Count > 2 ? targetQ.Options[2] : "";
                //txtOpt4.Text = targetQ.Options.Count > 3 ? targetQ.Options[3] : "";

                var correctIndex = choices.FindIndex(c => c.IsCorrect).ToString();

                if (ddlCorrectIndex.Items.FindByValue(correctIndex) != null)
                {
                    ddlCorrectIndex.SelectedValue = correctIndex;
                }

                // TODO ADD THIS
                //txtExplanation.Text = question.Explanation;

                litFormTitle.Text = "✏️ Edit Practice Question (" + question.Id + ")";
                btnAddQuestion.Text = "💾 Save Question Changes";
                btnCancelQuestionEdit.Visible = true;

                ShowNotification("Question " + question.Id + " loaded into editor below. Make your changes and click 'Save Question Changes'.");
            }

        }

        protected void handleDeleteQuestion(object sender, GridViewCommandEventArgs e)
        {
            int questionId = Convert.ToInt32(e.CommandArgument.ToString());

            using (var db = new AppDbContext())
            {
                var question = db.Questions.Find(questionId);
                if (question == null) return;

                db.Questions.Remove(question);
                db.SaveChanges();
                ShowNotification("Question deleted from database.");
                BindData();
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}