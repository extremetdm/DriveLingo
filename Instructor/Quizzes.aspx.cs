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

namespace DriveLingo.Instructor
{
    public partial class Quizzes : AuthPage
    {
        private int? ActiveQuizId
        {
            get => ViewState["ActiveQuizId"] as int?;
            set => ViewState["ActiveQuizId"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Instructor);

            if (!IsPostBack)
            {
                BindModuleDropdown();
                BindQuizzes();
            }
        }

        private void BindModuleDropdown()
        {
            using (var db = new AppDbContext())
            {
                ddlQuizModuleSection.DataSource = db.Modules.ToList();
                ddlQuizModuleSection.DataTextField = "Name";
                ddlQuizModuleSection.DataValueField = "ID";
                ddlQuizModuleSection.DataBind();
            }
            
            //UpdateModuleRateInfo();
        }

        protected void ddlQuizModuleSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            //UpdateModuleRateInfo();
        }

        //private void UpdateModuleRateInfo()
        //{
        //    string selectedModule = ddlQuizModuleSection.SelectedValue;
        //    var state = AppStateRepository.GetCurrent();
        //    var module = state.Modules.FirstOrDefault(m => m.Name.Equals(selectedModule, StringComparison.OrdinalIgnoreCase));

        //    int ptsRate = module != null ? module.RewardPointsPerQuestion : 20;
        //    litModuleRewardRateInfo.Text = "<span class='pts-rate-badge'>🪙 Admin Configured Rate: <strong>" + ptsRate + " Points</strong> awarded per question in this section</span>";
        //}

        private void BindQuizzes()
        {
            using (var db = new AppDbContext())
            {
                gvQuizzes.DataSource = db.Quizzes.ToList();
                gvQuizzes.DataBind();
            }
        }

        // --- EDUCATOR QUIZ CRUD HANDLERS ---
        protected void btnAddQuiz_Click(object sender, EventArgs e)
        {
            string title = txtQuizTitle.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                ShowNotification("Please enter a valid quiz title.");
                return;
            }

            string moduleId = ddlQuizModuleSection.SelectedValue;
            if (string.IsNullOrEmpty(moduleId))
            {
                ShowNotification("Please select a curriculum module section.");
                return;
            }

            using (var db = new AppDbContext())
            {
                var module = db.Modules.Find(Convert.ToInt32(moduleId));
                if (module == null)
                {
                    ShowNotification("Please select a curriculum module section.");
                    return;
                }

                string quizId = hfEditingQuizId.Value;
                Quiz quiz = null;
                if (!string.IsNullOrEmpty(quizId))
                {
                    quiz = db.Quizzes.Find(Convert.ToInt32(quizId));
                }

                bool isEdit = quiz != null;

                if (isEdit)
                {
                    quiz.ModuleId = module.Id;
                } else 
                {
                    quiz = new Quiz();
                    module.Quizzes.Add(quiz);
                } 

                quiz.Title = title;

                db.SaveChanges();

                if (isEdit)
                {
                    ShowNotification("Quiz '" + title + "' updated successfully under " + module.Name + "!");
                } else
                    ShowNotification("New quiz '" + title + "' created under " + module.Name + "! Now click '❓ Qs' to add questions.");

                ResetQuizForm();
                BindQuizzes();

            }
        }

        protected void btnCancelQuizEdit_Click(object sender, EventArgs e)
        {
            ResetQuizForm();
        }

        private void ResetQuizForm()
        {
            hfEditingQuizId.Value = "";
            txtQuizTitle.Text = "";
            litQuizFormTitle.Text = "➕ Create Quiz under Curriculum Module";
            btnAddQuiz.Text = "➕ Save Quiz";
            btnCancelQuizEdit.Visible = false;
        }

        protected void gvQuizzes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int quizId = Convert.ToInt32(e.CommandArgument.ToString());

            using (var db = new AppDbContext())
            {
                var quiz = db.Quizzes.Find(quizId);
                if (quiz == null)
                {
                    ShowNotification("No quiz selected.");
                    return;
                }

                if (e.CommandName == "EditQuiz")
                {
                    hfEditingQuizId.Value = quiz.Id.ToString();
                    txtQuizTitle.Text = quiz.Title;
                    if (ddlQuizModuleSection.Items.FindByValue(quiz.Module.Id.ToString()) != null)
                    {
                        ddlQuizModuleSection.SelectedValue = quiz.Module.Id.ToString();
                    }
                    //UpdateModuleRateInfo();
                    litQuizFormTitle.Text = "✏️ Edit Educator Quiz";
                    btnAddQuiz.Text = "💾 Save Changes";
                    btnCancelQuizEdit.Visible = true;
                }
                else if (e.CommandName == "DeleteQuiz")
                {
                    db.Quizzes.Remove(quiz);
                    if (ActiveQuizId == quizId)
                    {
                        pnlQuestionBank.Visible = false;
                        ActiveQuizId = null;
                    }
                    db.SaveChanges();
                    ShowNotification("Quiz deleted.");
                    BindQuizzes();
                }
                else if (e.CommandName == "ManageQuestions")
                {
                    ActiveQuizId = quiz.Id;
                    litActiveQuizTitle.Text = quiz.Title;
                    litActiveQuizModule.Text = quiz.Module.Name;
                    pnlQuestionBank.Visible = true;
                    ResetQuestionForm();
                    BindQuestions();
                }
            }
        }


        // --- QUESTION BANK AUTHORING HANDLERS ---
        private void BindQuestions()
        {
            using (var db = new AppDbContext())
            {
                gvQuestions.DataSource = db.Questions
                    //.Include(q => q.Quiz)
                    .Include(q => q.Choices)
                    .Where(q => q.QuizId == ActiveQuizId)
                    .ToList();
                gvQuestions.DataBind();
            }
        }

        protected void btnAddQuestion_Click(object sender, EventArgs e)
        {
            if (ActiveQuizId == null)
            {
                ShowNotification("No active quiz selected.");
                return;
            }

            string text = txtQuestionText.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                ShowNotification("Please provide question text.");
                return;
            }

            int correctIndex = Convert.ToInt32(ddlCorrectIndex.SelectedValue);

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

            if (choiceFilledCount < 2)
            {
                ShowNotification("Please provide question options.");
                return;
            }

            string imageUrl = txtQuestionImageUrl.Text.Trim();
            if (fileQuestionImage.HasFile && fileQuestionImage.FileContent.Length > 0)
            {
                var output = UploadService.UploadImage(fileQuestionImage);
                if (!output.Success)
                {
                    ShowNotification(output.Message);
                    return;
                }
                imageUrl = output.FilePath;
            }

            //string explanation = txtExplanation.Text.Trim();


            using (var db = new AppDbContext())
            {
                Question question = null;
                string questionId = hfEditingQuestionId.Value;
                if (!string.IsNullOrEmpty(questionId))
                {
                    question = db.Questions.Find(Convert.ToInt32(questionId));
                }

                var quiz = db.Quizzes.Find(ActiveQuizId);
                if (quiz == null)
                {
                    ShowNotification("No active quiz selected.");
                    return;
                }

                bool isEdit = question != null;

                if (isEdit)
                {
                    question.QuizId = quiz.Id;
                    question.Text = text;
                    if (!string.IsNullOrEmpty(imageUrl)) question.Image = imageUrl;
                }
                else
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
                BindQuestions();
                BindQuizzes();
            }
        }

        protected void btnCancelQuestionEdit_Click(object sender, EventArgs e)
        {
            ResetQuestionForm();
        }

        protected void btnCloseQuestionBank_Click(object sender, EventArgs e)
        {
            pnlQuestionBank.Visible = false;
            ActiveQuizId = null;
        }

        private void ResetQuestionForm()
        {
            hfEditingQuestionId.Value = "";
            txtQuestionText.Text = "";
            txtQuestionImageUrl.Text = "";
            txtQuestionExplanation.Text = "";
            setChoices(new List<Choice>());
            ddlCorrectIndex.SelectedValue = "0";
            litQuestionFormTitle.Text = "➕ Add New Question";
            btnAddQuestion.Text = "➕ Save Question";
            btnCancelQuestionEdit.Visible = false;
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
                txtQuestionText.Text = question.Text;
                txtQuestionImageUrl.Text = question.Image;
                //txtQuestionExplanation.Text = question.Explanation;

                var choices = question.Choices.ToList();

                setChoices(choices.Select(c => new Choice
                {
                    Id = c.Id,
                    Text = c.Text
                }).ToList());

                var correctIndex = choices.FindIndex(c => c.IsCorrect).ToString();
                if (ddlCorrectIndex.Items.FindByValue(correctIndex) != null)
                {
                    ddlCorrectIndex.SelectedValue = correctIndex;
                }

                // TODO ADD THIS
                //txtExplanation.Text = question.Explanation;

                litQuestionFormTitle.Text = "✏️ Edit Question";
                btnAddQuestion.Text = "💾 Update Question";
                btnCancelQuestionEdit.Visible = true;

                //ShowNotification("Question " + question.Id + " loaded into editor below. Make your changes and click 'Save Question Changes'.");
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
                BindQuestions();
                BindQuizzes(); 
            }
        }
        struct Choice
        {
            public int? Id { get; set; }
            public string Text { get; set; }

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



        // --- VIEW HELPERS ---
        public int GetQuestionCount(object questionsObj)
        {
            if (questionsObj is List<Question> list)
            {
                return list.Count;
            }
            return 0;
        }

        public char GetChoicePlaceholder(int index)
        {
            return (char)('A' + index);
        }


        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = message;
        }
    }
}