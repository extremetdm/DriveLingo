using DriveLingo.Data;
using DriveLingo.Models;
using DriveLingo.Services;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo.Instructor
{
    public partial class Quizzes : AuthPage
    {
        private string ActiveQuizId
        {
            get => ViewState["ActiveQuizId"] as string ?? "";
            set => ViewState["ActiveQuizId"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Instructor);

            if (!IsPostBack)
            {
                EnsureModuleDataSeeded();
                BindModuleDropdown();
                BindQuizzes();
            }
        }

        private void EnsureModuleDataSeeded()
        {
            var state = AppStateRepository.GetCurrent();
            if (state.Modules == null || state.Modules.Count == 0)
            {
                state.Modules = new List<ModuleItem>
                {
                    new ModuleItem { Id = "mod_sec_a", Name = "Section A - Road Signs", Description = "Prohibitory, warning, and mandatory road sign regulations.", Icon = "🛑", RewardPointsPerQuestion = 20 },
                    new ModuleItem { Id = "mod_sec_b", Name = "Section B - Rules of the Road", Description = "Speed limits, lane discipline, traffic signals, and right of way.", Icon = "🚗", RewardPointsPerQuestion = 25 },
                    new ModuleItem { Id = "mod_sec_c", Name = "Section C - KEJARA & Safety", Description = "Demerit point penalties, alcohol laws, and emergency procedures.", Icon = "🚦", RewardPointsPerQuestion = 30 },
                    new ModuleItem { Id = "mod_cb", Name = "Color Blind", Description = "Official Ishihara color vision screening plates.", Icon = "👁️", RewardPointsPerQuestion = 15 }
                };
            }
        }

        private void BindModuleDropdown()
        {
            var state = AppStateRepository.GetCurrent();
            ddlQuizModuleSection.DataSource = state.Modules;
            ddlQuizModuleSection.DataTextField = "Name";
            ddlQuizModuleSection.DataValueField = "Name";
            ddlQuizModuleSection.DataBind();

            UpdateModuleRateInfo();
        }

        protected void ddlQuizModuleSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateModuleRateInfo();
        }

        private void UpdateModuleRateInfo()
        {
            string selectedModule = ddlQuizModuleSection.SelectedValue;
            var state = AppStateRepository.GetCurrent();
            var module = state.Modules.FirstOrDefault(m => m.Name.Equals(selectedModule, StringComparison.OrdinalIgnoreCase));

            int ptsRate = module != null ? module.RewardPointsPerQuestion : 20;
            litModuleRewardRateInfo.Text = "<span class='pts-rate-badge'>🪙 Admin Configured Rate: <strong>" + ptsRate + " Points</strong> awarded per question in this section</span>";
        }

        private void BindQuizzes()
        {
            var state = AppStateRepository.GetCurrent();
            gvQuizzes.DataSource = state.Quizzes;
            gvQuizzes.DataBind();
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

            string selectedModule = ddlQuizModuleSection.SelectedValue;
            if (string.IsNullOrEmpty(selectedModule))
            {
                ShowNotification("Please select a curriculum module section.");
                return;
            }

            var state = AppStateRepository.GetCurrent();
            var module = state.Modules.FirstOrDefault(m => m.Name.Equals(selectedModule, StringComparison.OrdinalIgnoreCase));
            int ptsPerQ = module != null ? module.RewardPointsPerQuestion : 20;

            string editingId = hfEditingQuizId.Value;

            if (!string.IsNullOrEmpty(editingId))
            {
                var quizToEdit = state.Quizzes.FirstOrDefault(q => q.Id == editingId);
                if (quizToEdit != null)
                {
                    quizToEdit.Title = title;
                    quizToEdit.Category = selectedModule;
                    quizToEdit.RewardPoints = quizToEdit.Questions.Count * ptsPerQ;
                    ShowNotification("Quiz '" + title + "' updated successfully under " + selectedModule + "!");
                }
            }
            else
            {
                var newQuiz = new Quiz
                {
                    Id = "quiz_edu_" + Guid.NewGuid().ToString().Substring(0, 8),
                    Title = title,
                    Category = selectedModule,
                    RewardPoints = 0,
                    Questions = new List<Question>()
                };
                state.Quizzes.Add(newQuiz);
                ShowNotification("New quiz '" + title + "' created under " + selectedModule + "! Now click '❓ Qs' to add questions.");
            }

            ResetQuizForm();
            BindQuizzes();
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
            string quizId = e.CommandArgument.ToString();
            var state = AppStateRepository.GetCurrent();
            var quiz = state.Quizzes.FirstOrDefault(q => q.Id == quizId);

            if (e.CommandName == "EditQuiz")
            {
                if (quiz != null)
                {
                    hfEditingQuizId.Value = quiz.Id;
                    txtQuizTitle.Text = quiz.Title;
                    if (ddlQuizModuleSection.Items.FindByValue(quiz.Category) != null)
                    {
                        ddlQuizModuleSection.SelectedValue = quiz.Category;
                    }
                    UpdateModuleRateInfo();
                    litQuizFormTitle.Text = "✏️ Edit Educator Quiz";
                    btnAddQuiz.Text = "💾 Save Changes";
                    btnCancelQuizEdit.Visible = true;
                }
            }
            else if (e.CommandName == "DeleteQuiz")
            {
                if (quiz != null)
                {
                    state.Quizzes.Remove(quiz);
                    if (ActiveQuizId == quizId)
                    {
                        pnlQuestionBank.Visible = false;
                        ActiveQuizId = "";
                    }
                    ShowNotification("Quiz deleted.");
                    BindQuizzes();
                }
            }
            else if (e.CommandName == "ManageQuestions")
            {
                if (quiz != null)
                {
                    ActiveQuizId = quiz.Id;
                    litActiveQuizTitle.Text = quiz.Title;
                    litActiveQuizModule.Text = quiz.Category;
                    pnlQuestionBank.Visible = true;
                    ResetQuestionForm();
                    BindQuestions();
                }
            }
        }

        // --- QUESTION BANK AUTHORING HANDLERS ---
        private void BindQuestions()
        {
            var state = AppStateRepository.GetCurrent();
            var quiz = state.Quizzes.FirstOrDefault(q => q.Id == ActiveQuizId);

            if (quiz != null)
            {
                gvQuestions.DataSource = quiz.Questions;
                gvQuestions.DataBind();
            }
        }

        protected void btnAddQuestion_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ActiveQuizId))
            {
                ShowNotification("No active quiz selected.");
                return;
            }

            string text = txtQuestionText.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                ShowNotification("Please provide question prompt text.");
                return;
            }

            string optA = txtOptionA.Text.Trim();
            string optB = txtOptionB.Text.Trim();
            string optC = txtOptionC.Text.Trim();
            string optD = txtOptionD.Text.Trim();

            if (string.IsNullOrEmpty(optA) || string.IsNullOrEmpty(optB))
            {
                ShowNotification("Please provide at least Option A and Option B.");
                return;
            }

            var options = new List<string> { optA, optB };
            if (!string.IsNullOrEmpty(optC)) options.Add(optC);
            if (!string.IsNullOrEmpty(optD)) options.Add(optD);

            int correctIndex = Convert.ToInt32(ddlCorrectIndex.SelectedValue);
            if (correctIndex >= options.Count)
            {
                correctIndex = 0;
            }

            string imageUrl = txtQuestionImageUrl.Text.Trim();
            if (fileQuestionImage.HasFile)
            {
                var uploadResult = UploadService.UploadImage(fileQuestionImage);
                if (uploadResult.Success)
                {
                    imageUrl = uploadResult.FilePath;
                }
            }

            var state = AppStateRepository.GetCurrent();
            var quiz = state.Quizzes.FirstOrDefault(q => q.Id == ActiveQuizId);
            if (quiz == null) return;

            var module = state.Modules.FirstOrDefault(m => m.Name.Equals(quiz.Category, StringComparison.OrdinalIgnoreCase));
            int ptsPerQ = module != null ? module.RewardPointsPerQuestion : 20;

            string editingQId = hfEditingQuestionId.Value;

            if (!string.IsNullOrEmpty(editingQId))
            {
                var qToEdit = quiz.Questions.FirstOrDefault(q => q.Id == editingQId);
                if (qToEdit != null)
                {
                    qToEdit.Text = text;
                    qToEdit.Options = options;
                    qToEdit.CorrectIndex = correctIndex;
                    qToEdit.Explanation = txtQuestionExplanation.Text.Trim();
                    qToEdit.ImageUrl = imageUrl;
                    ShowNotification("Question updated successfully!");
                }
            }
            else
            {
                var newQ = new Question
                {
                    Id = "q_" + Guid.NewGuid().ToString().Substring(0, 8),
                    QuizId = quiz.Id,
                    Text = text,
                    Options = options,
                    CorrectIndex = correctIndex,
                    Explanation = txtQuestionExplanation.Text.Trim(),
                    ImageUrl = imageUrl,
                    Section = quiz.Category
                };
                quiz.Questions.Add(newQ);
                ShowNotification("New question added to quiz!");
            }

            // Recalculate quiz total reward points using Admin's rate per question
            quiz.RewardPoints = quiz.Questions.Count * ptsPerQ;

            ResetQuestionForm();
            BindQuestions();
            BindQuizzes();
        }

        protected void btnCancelQuestionEdit_Click(object sender, EventArgs e)
        {
            ResetQuestionForm();
        }

        protected void btnCloseQuestionBank_Click(object sender, EventArgs e)
        {
            pnlQuestionBank.Visible = false;
            ActiveQuizId = "";
        }

        private void ResetQuestionForm()
        {
            hfEditingQuestionId.Value = "";
            txtQuestionText.Text = "";
            txtOptionA.Text = "";
            txtOptionB.Text = "";
            txtOptionC.Text = "";
            txtOptionD.Text = "";
            txtQuestionImageUrl.Text = "";
            txtQuestionExplanation.Text = "";
            ddlCorrectIndex.SelectedValue = "0";
            litQuestionFormTitle.Text = "➕ Add New Question";
            btnAddQuestion.Text = "➕ Save Question";
            btnCancelQuestionEdit.Visible = false;
        }

        protected void gvQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string qId = e.CommandArgument.ToString();
            var state = AppStateRepository.GetCurrent();
            var quiz = state.Quizzes.FirstOrDefault(q => q.Id == ActiveQuizId);
            if (quiz == null) return;

            var question = quiz.Questions.FirstOrDefault(q => q.Id == qId);

            if (e.CommandName == "EditQuestion")
            {
                if (question != null)
                {
                    hfEditingQuestionId.Value = question.Id;
                    txtQuestionText.Text = question.Text;
                    txtQuestionImageUrl.Text = question.ImageUrl;
                    txtQuestionExplanation.Text = question.Explanation;

                    txtOptionA.Text = question.Options.Count > 0 ? question.Options[0] : "";
                    txtOptionB.Text = question.Options.Count > 1 ? question.Options[1] : "";
                    txtOptionC.Text = question.Options.Count > 2 ? question.Options[2] : "";
                    txtOptionD.Text = question.Options.Count > 3 ? question.Options[3] : "";

                    if (ddlCorrectIndex.Items.FindByValue(question.CorrectIndex.ToString()) != null)
                    {
                        ddlCorrectIndex.SelectedValue = question.CorrectIndex.ToString();
                    }

                    litQuestionFormTitle.Text = "✏️ Edit Question";
                    btnAddQuestion.Text = "💾 Update Question";
                    btnCancelQuestionEdit.Visible = true;
                }
            }
            else if (e.CommandName == "DeleteQuestion")
            {
                if (question != null)
                {
                    quiz.Questions.Remove(question);
                    
                    var module = state.Modules.FirstOrDefault(m => m.Name.Equals(quiz.Category, StringComparison.OrdinalIgnoreCase));
                    int ptsPerQ = module != null ? module.RewardPointsPerQuestion : 20;
                    quiz.RewardPoints = quiz.Questions.Count * ptsPerQ;

                    ShowNotification("Question deleted.");
                    BindQuestions();
                    BindQuizzes();
                }
            }
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

        public int CalculateQuizRewardPoints(string category, object questionsObj)
        {
            int qCount = GetQuestionCount(questionsObj);
            var state = AppStateRepository.GetCurrent();
            var module = state.Modules.FirstOrDefault(m => m.Name.Equals(category, StringComparison.OrdinalIgnoreCase));
            int ptsPerQ = module != null ? module.RewardPointsPerQuestion : 20;

            return qCount * ptsPerQ;
        }

        public string GetOptionLetter(object indexObj)
        {
            if (indexObj != null && int.TryParse(indexObj.ToString(), out int idx))
            {
                if (idx >= 0 && idx < 4)
                {
                    return ((char)('A' + idx)).ToString();
                }
            }
            return "A";
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = message;
        }
    }
}