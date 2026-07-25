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
    public partial class Achievements : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Admin);

            if (!IsPostBack)
            {
                BindAchievements();
            }
        }

        private void BindAchievements()
        {
            var state = AppStateRepository.GetCurrent();
            gvAchievements.DataSource = state.Achievements;
            gvAchievements.DataBind();
        }

        protected void btnAddAch_Click(object sender, EventArgs e)
        {
            string title = txtAchTitle.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                ShowNotification("Please enter achievement title.");
                return;
            }

            string icon = txtAchIcon.Text.Trim();
            if (string.IsNullOrEmpty(icon)) icon = "🏆";

            int xpBonus = 100;
            int.TryParse(txtAchXp.Text.Trim(), out xpBonus);

            int targetCount = 5;
            int.TryParse(txtTargetCount.Text.Trim(), out targetCount);
            if (targetCount <= 0) targetCount = 1;

            string description = txtAchDesc.Text.Trim();

            var state = AppStateRepository.GetCurrent();
            string editingId = hfEditingAchId.Value;

            if (!string.IsNullOrEmpty(editingId))
            {
                var achToEdit = state.Achievements.FirstOrDefault(a => a.Id == editingId);
                if (achToEdit != null)
                {
                    achToEdit.Title = title;
                    achToEdit.Icon = icon;
                    achToEdit.XpBonus = xpBonus;
                    achToEdit.TargetCount = targetCount;
                    achToEdit.Description = description;

                    ShowNotification("Achievement '" + title + "' updated successfully!");
                }
            }
            else
            {
                var newAch = new Achievement
                {
                    Id = "ach_" + Guid.NewGuid().ToString().Substring(0, 8),
                    Title = title,
                    Icon = icon,
                    XpBonus = xpBonus,
                    TargetCount = targetCount,
                    MetricType = "quiz_count",
                    Description = description
                };
                state.Achievements.Add(newAch);
                ShowNotification("New achievement '" + title + "' created successfully!");
            }

            ResetAchForm();
            BindAchievements();
        }

        protected void btnCancelAchEdit_Click(object sender, EventArgs e)
        {
            ResetAchForm();
        }

        private void ResetAchForm()
        {
            hfEditingAchId.Value = "";
            txtAchTitle.Text = "";
            txtAchIcon.Text = "🏆";
            txtAchXp.Text = "100";
            txtTargetCount.Text = "5";
            txtAchDesc.Text = "";
            litAchFormTitle.Text = "➕ Create Achievement";
            btnAddAch.Text = "➕ Create Achievement";
            btnCancelAchEdit.Visible = false;
        }

        protected void gvAchievements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string achId = e.CommandArgument.ToString();
            var state = AppStateRepository.GetCurrent();
            var achievement = state.Achievements.FirstOrDefault(a => a.Id == achId);

            if (e.CommandName == "EditAchievement")
            {
                if (achievement != null)
                {
                    hfEditingAchId.Value = achievement.Id;
                    txtAchTitle.Text = achievement.Title;
                    txtAchIcon.Text = achievement.Icon;
                    txtAchXp.Text = achievement.XpBonus.ToString();
                    txtTargetCount.Text = achievement.TargetCount.ToString();
                    txtAchDesc.Text = achievement.Description;

                    litAchFormTitle.Text = "✏️ Edit Achievement";
                    btnAddAch.Text = "💾 Save Changes";
                    btnCancelAchEdit.Visible = true;
                }
            }
            else if (e.CommandName == "DeleteAchievement")
            {
                if (achievement != null)
                {
                    state.Achievements.Remove(achievement);
                    ShowNotification("Achievement '" + achievement.Title + "' deleted.");
                    ResetAchForm();
                    BindAchievements();
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