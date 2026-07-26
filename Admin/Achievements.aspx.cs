using DriveLingo.Database;
using DriveLingo.Database.Models;
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
            using (var db = new AppDbContext())
            {
                gvAchievements.DataSource = db.Achievements.ToList();
                gvAchievements.DataBind();
            }
        }

        protected void btnAddAch_Click(object sender, EventArgs e)
        {
            string name = txtAchTitle.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowNotification("Please enter achievement name.");
                return;
            }

            string icon = txtAchIcon.Text.Trim();
            if (string.IsNullOrEmpty(icon))
            {
                ShowNotification("Please enter achievement icon.");
                return;
            }

            string description = txtAchDesc.Text.Trim();
            if (string.IsNullOrEmpty(description))
            {
                ShowNotification("Please enter achievement description.");
                return;
            }

            int xp;
            if (!int.TryParse(txtAchXp.Text.Trim(), out xp))
            {
                ShowNotification("Please enter XP bonus.");
                return;
            }


            int target;
            if (!int.TryParse(txtTargetCount.Text.Trim(), out target))
            {
                ShowNotification("Please enter target.");
                return;
            }

            Achievement.TaskType task;
            if (!Enum.TryParse(ddlMetricType.SelectedValue, out task))
            {
                ShowNotification("Please select task type.");
                return;
            }

            using (var db = new AppDbContext())
            {
                string achievementId = hfEditingAchId.Value;
                Achievement achievement = null;
                if (!string.IsNullOrEmpty(achievementId))
                {
                    achievement = db.Achievements.Find(Convert.ToInt32(achievementId));
                }
                bool isEdit = achievement != null;
                if (!isEdit)
                {
                    achievement = new Achievement();
                    db.Achievements.Add(achievement);
                }
                achievement.Name = name;
                achievement.Icon = icon;
                achievement.Description = description;
                achievement.Xp = xp;
                achievement.Points = 0; // TODO CHANGE THESE maybe
                achievement.Target = target;
                achievement.Task = task;

                db.SaveChanges();

                if (isEdit)
                {
                    ShowNotification("Achievement " + name + " updated successfully!");
                }
                else
                {
                    ShowNotification("Achievement " + name + " created successfully!");
                }
                ResetAchForm();
                BindAchievements();
            }
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
            ddlMetricType.SelectedValue = "quiz_count";
            txtAchDesc.Text = "";
            litAchFormTitle.Text = "➕ Create Achievement";
            btnAddAch.Text = "➕ Create Achievement";
            btnCancelAchEdit.Visible = false;
        }

        protected void gvAchievements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditAchievement")
            {
                handleEdit(sender, e);
            }
            else if (e.CommandName == "DeleteAchievement")
            {
                handleDelete(sender, e);
            }
        }

        protected void handleEdit(object sender, GridViewCommandEventArgs e)
        {
            var achievementId = Convert.ToInt32(e.CommandArgument.ToString());

            using (var db = new AppDbContext())
            {
                var achievement = db.Achievements.Find(achievementId);
                if (achievement == null)
                {
                    ShowNotification("Achievement not found.");
                    return;
                }

                hfEditingAchId.Value = achievement.Id.ToString();
                txtAchTitle.Text = achievement.Name;
                txtAchIcon.Text = achievement.Icon;
                txtAchXp.Text = achievement.Xp.ToString();
                txtAchDesc.Text = achievement.Description;
                
                txtTargetCount.Text = achievement.Target.ToString();

                if (ddlMetricType.Items.FindByValue(achievement.Task.ToString()) != null)
                {
                    ddlMetricType.SelectedValue = achievement.Task.ToString();
                }

                litAchFormTitle.Text = "✏️ Edit Achievement";
                btnAddAch.Text = "💾 Save Changes";
                btnCancelAchEdit.Visible = true;

            }
        }

        protected void handleDelete(object sender, GridViewCommandEventArgs e)
        {
            var achievementId = Convert.ToInt32(e.CommandArgument.ToString());

            using (var db = new AppDbContext())
            {
                var achievement = db.Achievements.Find(achievementId);
                if (achievement == null)
                {
                    ShowNotification("Achievement not found.");
                    return;
                }

                db.Achievements.Remove(achievement);
                db.SaveChanges();
                ShowNotification("Achievement deleted.");
                BindAchievements();
            }
        }

        

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = message;
        }
    }
}