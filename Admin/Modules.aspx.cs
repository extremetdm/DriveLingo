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
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Admin);

            if (!IsPostBack)
            {
                EnsureModuleDataSeeded();
                BindModules();
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

        private void BindModules()
        {
            var state = AppStateRepository.GetCurrent();
            gvModules.DataSource = state.Modules;
            gvModules.DataBind();
        }

        // --- MODULE CRUD HANDLERS (ADMIN SETS MODULES & REWARD POINTS PER QUESTION) ---
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

            int ptsPerQ = 20;
            int.TryParse(txtRewardPointsPerQuestion.Text.Trim(), out ptsPerQ);
            if (ptsPerQ <= 0) ptsPerQ = 20;

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
                    modToEdit.RewardPointsPerQuestion = ptsPerQ;
                    modToEdit.Description = description;

                    // Update existing quizzes under old module name if renamed
                    foreach (var q in state.Quizzes.Where(qz => qz.Category == oldName))
                    {
                        q.Category = name;
                    }

                    ShowNotification("Module '" + name + "' updated with " + ptsPerQ + " Pts/Question rate!");
                }
            }
            else
            {
                var newModule = new ModuleItem
                {
                    Id = "mod_" + Guid.NewGuid().ToString().Substring(0, 8),
                    Name = name,
                    Icon = icon,
                    RewardPointsPerQuestion = ptsPerQ,
                    Description = description
                };
                state.Modules.Add(newModule);
                ShowNotification("New curriculum module '" + name + "' created with " + ptsPerQ + " Pts/Question rate!");
            }

            ResetModuleForm();
            BindModules();
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
            txtRewardPointsPerQuestion.Text = "20";
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
                    txtRewardPointsPerQuestion.Text = module.RewardPointsPerQuestion.ToString();
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
                }
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = message;
        }
    }
}
