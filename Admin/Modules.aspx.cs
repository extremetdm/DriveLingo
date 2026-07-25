using DriveLingo.Data;
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
    public partial class Modules : AuthPage
    {
        public string ActiveFilterCategory
        {
            get => ViewState["ActiveFilterCategory"] as string ?? "ALL";
            set => ViewState["ActiveFilterCategory"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Admin);

            if (!IsPostBack)
            {
                EnsureModuleDataSeeded();
                BindModules();
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
                    new ModuleItem { Id = "mod_sec_a", Name = "Section A - Road Signs", Description = "Prohibitory, warning, and mandatory road sign regulations.", Icon = "🛑" },
                    new ModuleItem { Id = "mod_sec_b", Name = "Section B - Rules of the Road", Description = "Speed limits, lane discipline, traffic signals, and right of way.", Icon = "🚗" },
                    new ModuleItem { Id = "mod_sec_c", Name = "Section C - KEJARA & Safety", Description = "Demerit point penalties, alcohol laws, and emergency procedures.", Icon = "🚦" },
                    new ModuleItem { Id = "mod_cb", Name = "Color Blind", Description = "Official Ishihara color vision screening plates.", Icon = "👁️" }
                };
            }

            if (state.Quizzes == null)
            {
                state.Quizzes = new List<Quiz>();
            }

            if (state.Quizzes.Count == 0)
            {
                state.Quizzes.Add(new Quiz
                {
                    Id = "quiz_sec_a_1",
                    Title = "Prohibitory Road Signs Test",
                    Category = "Section A - Road Signs",
                    RewardPoints = 100,
                    Questions = new List<Question>
                    {
                        new Question { Id = "q_1", Text = "What does a circular red border sign indicate?", Options = new List<string> { "Command", "Prohibition", "Warning", "Guide" }, CorrectIndex = 1 }
                    }
                });

                state.Quizzes.Add(new Quiz
                {
                    Id = "quiz_sec_b_1",
                    Title = "Expressway Speed Limits Test",
                    Category = "Section B - Rules of the Road",
                    RewardPoints = 120,
                    Questions = new List<Question>
                    {
                        new Question { Id = "q_2", Text = "What is the legal expressway speed limit?", Options = new List<string> { "90 km/h", "100 km/h", "110 km/h", "120 km/h" }, CorrectIndex = 2 }
                    }
                });
            }
        }

        private void BindModules()
        {
            var state = AppStateRepository.GetCurrent();
            
            // Bind Modules Grid
            gvModules.DataSource = state.Modules;
            gvModules.DataBind();

            // Populate Quiz Target Module Dropdown dynamically
            ddlQuizModule.DataSource = state.Modules;
            ddlQuizModule.DataTextField = "Name";
            ddlQuizModule.DataValueField = "Name";
            ddlQuizModule.DataBind();

            // Populate Module Filter Buttons
            rptModuleFilters.DataSource = state.Modules;
            rptModuleFilters.DataBind();
        }

        private void BindQuizzes()
        {
            var state = AppStateRepository.GetCurrent();
            var quizzes = state.Quizzes.AsEnumerable();

            if (ActiveFilterCategory != "ALL")
            {
                quizzes = quizzes.Where(q => q.Category.Equals(ActiveFilterCategory, StringComparison.OrdinalIgnoreCase));
            }

            gvQuizzes.DataSource = quizzes.ToList();
            gvQuizzes.DataBind();
        }

        // --- DYNAMIC MODULE CRUD HANDLERS (ADD, EDIT, DELETE MODULES) ---
        protected void btnAddModule_Click(object sender, EventArgs e)
        {
            string name = txtModuleName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowNotification("Please provide a module name.");
                return;
            }

            string icon = txtModuleIcon.Text.Trim();
            if (string.IsNullOrEmpty(icon)) icon = "📁";

            string description = txtModuleDescription.Text.Trim();

            var state = AppStateRepository.GetCurrent();
            string editingId = hfEditingModuleId.Value;

            if (!string.IsNullOrEmpty(editingId))
            {
                var modToEdit = state.Modules.FirstOrDefault(m => m.Id == editingId);
                if (modToEdit != null)
                {
                    string oldName = modToEdit.Name;
                    modToEdit.Name = name;
                    modToEdit.Icon = icon;
                    modToEdit.Description = description;

                    // Update existing quizzes under old module name
                    foreach (var q in state.Quizzes.Where(qz => qz.Category == oldName))
                    {
                        q.Category = name;
                    }

                    ShowNotification("Module '" + name + "' updated successfully!");
                }
            }
            else
            {
                var newModule = new ModuleItem
                {
                    Id = "mod_" + Guid.NewGuid().ToString().Substring(0, 8),
                    Name = name,
                    Icon = icon,
                    Description = description
                };
                state.Modules.Add(newModule);
                ShowNotification("New curriculum module '" + name + "' created successfully!");
            }

            ResetModuleForm();
            BindModules();
            BindQuizzes();
        }

        protected void btnCancelModuleEdit_Click(object sender, EventArgs e)
        {
            ResetModuleForm();
        }

        private void ResetModuleForm()
        {
            hfEditingModuleId.Value = "";
            txtModuleName.Text = "";
            txtModuleIcon.Text = "📁";
            txtModuleDescription.Text = "";
            litModuleFormTitle.Text = "➕ Create New Curriculum Module";
            btnAddModule.Text = "➕ Save Module";
            btnCancelModuleEdit.Visible = false;
        }

        protected void gvModules_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string modId = e.CommandArgument.ToString();
            var state = AppStateRepository.GetCurrent();
            var module = state.Modules.FirstOrDefault(m => m.Id == modId);

            if (e.CommandName == "EditModule")
            {
                if (module != null)
                {
                    hfEditingModuleId.Value = module.Id;
                    txtModuleName.Text = module.Name;
                    txtModuleIcon.Text = module.Icon;
                    txtModuleDescription.Text = module.Description;
                    litModuleFormTitle.Text = "✏️ Edit Curriculum Module";
                    btnAddModule.Text = "💾 Save Module Changes";
                    btnCancelModuleEdit.Visible = true;
                }
            }
            else if (e.CommandName == "DeleteModule")
            {
                if (module != null)
                {
                    state.Modules.Remove(module);
                    ShowNotification("Module '" + module.Name + "' deleted.");
                    ResetModuleForm();
                    BindModules();
                    BindQuizzes();
                }
            }
        }

        // --- FILTER TAB HANDLERS ---
        protected void btnFilterAllModules_Click(object sender, EventArgs e)
        {
            ActiveFilterCategory = "ALL";
            btnFilterAllModules.CssClass = "module-tab-btn active";
            BindQuizzes();
        }

        protected void rptModuleFilters_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "FilterByModule")
            {
                ActiveFilterCategory = e.CommandArgument.ToString();
                btnFilterAllModules.CssClass = "module-tab-btn";
                BindQuizzes();
                BindModules(); // Rebind repeater to refresh active CSS classes
            }
        }

        public string GetFilterTabClass(string moduleName)
        {
            if (ActiveFilterCategory.Equals(moduleName, StringComparison.OrdinalIgnoreCase))
            {
                return "module-tab-btn active";
            }
            return "module-tab-btn";
        }

        // --- QUIZ CRUD HANDLERS ---
        protected void btnAddQuiz_Click(object sender, EventArgs e)
        {
            string title = txtQuizTitle.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                ShowNotification("Please enter a valid quiz title.");
                return;
            }

            string selectedModule = ddlQuizModule.SelectedValue;
            if (string.IsNullOrEmpty(selectedModule))
            {
                ShowNotification("Please select a target module.");
                return;
            }

            int rewardPoints = 100;
            int.TryParse(txtQuizRewardPoints.Text.Trim(), out rewardPoints);

            var state = AppStateRepository.GetCurrent();
            string editingId = hfEditingQuizId.Value;

            if (!string.IsNullOrEmpty(editingId))
            {
                var quizToEdit = state.Quizzes.FirstOrDefault(q => q.Id == editingId);
                if (quizToEdit != null)
                {
                    quizToEdit.Title = title;
                    quizToEdit.Category = selectedModule;
                    quizToEdit.RewardPoints = rewardPoints;
                    ShowNotification("Quiz '" + title + "' updated successfully!");
                }
            }
            else
            {
                var newQuiz = new Quiz
                {
                    Id = "quiz_mod_" + Guid.NewGuid().ToString().Substring(0, 8),
                    Title = title,
                    Category = selectedModule,
                    RewardPoints = rewardPoints,
                    Questions = new List<Question>()
                };
                state.Quizzes.Add(newQuiz);
                ShowNotification("New quiz '" + title + "' created under " + selectedModule + "!");
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
            txtQuizRewardPoints.Text = "100";
            litQuizFormTitle.Text = "➕ Add Quiz under Module";
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
                    txtQuizRewardPoints.Text = quiz.RewardPoints.ToString();
                    if (ddlQuizModule.Items.FindByValue(quiz.Category) != null)
                    {
                        ddlQuizModule.SelectedValue = quiz.Category;
                    }
                    litQuizFormTitle.Text = "✏️ Edit Quiz";
                    btnAddQuiz.Text = "💾 Save Changes";
                    btnCancelQuizEdit.Visible = true;
                }
            }
            else if (e.CommandName == "DeleteQuiz")
            {
                if (quiz != null)
                {
                    state.Quizzes.Remove(quiz);
                    ShowNotification("Quiz deleted.");
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

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = message;
        }
    }
}
