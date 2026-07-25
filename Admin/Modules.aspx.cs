using DriveLingo.Database;
using DriveLingo.Database.Models;
using DriveLingo.Services;
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
                BindModules();
            }
        }
        
        private void BindModules()
        {
            gvModules.DataSource = ModuleService.GetModules();
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

            //string icon = txtModuleIcon.Text.Trim();
            //if (string.IsNullOrEmpty(icon)) icon = "📁";

            //int ptsPerQ = 20;
            //int.TryParse(txtRewardPointsPerQuestion.Text.Trim(), out ptsPerQ);
            //if (ptsPerQ <= 0) ptsPerQ = 20;

            string description = txtModuleDescription.Text.Trim();

            using (var db = new AppDbContext())
            {
                string moduleId = hfEditingModuleId.Value;
                Module module = null;
                if (!string.IsNullOrEmpty(moduleId))
                {
                    module = db.Modules.Find(moduleId);
                }

                bool isEdit = module != null;
                string oldName;

                if (isEdit)
                {
                    oldName = module.Name;
                }
                else
                {
                    module = new Module();
                    db.Modules.Add(module);
                }


                module.Name = name;
                //module.Icon = icon;

                //module.RewardPointsPerQuestion = ptsPerQ;
                module.Description = description;

                db.SaveChanges();

                if (isEdit)
                {
                    ShowNotification("Module '" + name + "' updated.");

                } else
                {
                    ShowNotification("New curriculum module '" + name + "' added.");
                }
                ResetModuleForm();
                BindModules();
            }
        }

        protected void btnCancelModuleEdit_Click(object sender, EventArgs e)
        {
            ResetModuleForm();
        }

        private void ResetModuleForm()
        {
            hfEditingModuleId.Value = "";
            txtModuleName.Text = "";
            //txtModuleIcon.Text = "📁";
            //txtRewardPointsPerQuestion.Text = "20";
            txtModuleDescription.Text = "";
            litModuleFormTitle.Text = "➕ Create New Curriculum Module";
            btnAddModule.Text = "➕ Save Module";
            btnCancelModuleEdit.Visible = false;
        }

        protected void gvModules_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int moduleId = Convert.ToInt32(e.CommandArgument.ToString());

            using (var db = new AppDbContext())
            {
                var module = db.Modules.Find(moduleId);

                if (module == null)
                {
                    ShowNotification("Module not found.");
                    return;
                }

                if (e.CommandName == "EditModule")
                {
                    hfEditingModuleId.Value = module.Id.ToString();
                    txtModuleName.Text = module.Name;
                    //txtModuleIcon.Text = module.Icon;
                    //txtRewardPointsPerQuestion.Text = module.RewardPointsPerQuestion.ToString();
                    txtModuleDescription.Text = module.Description;
                    litModuleFormTitle.Text = "✏️ Edit Curriculum Module";
                    btnAddModule.Text = "💾 Save Module Changes";
                    btnCancelModuleEdit.Visible = true;
                }
                else if (e.CommandName == "DeleteModule")
                {
                    db.Modules.Remove(module);
                    db.SaveChanges();
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
