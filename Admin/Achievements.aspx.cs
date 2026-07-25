using DriveLingo.Data;
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

        // --- Achievements CRUD Handlers ---
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
            // TODO ADD TASKS, POINTS, TARGET

            int xp;
            int.TryParse(txtAchXp.Text.Trim(), out xp);

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
                achievement.Points = 0; // TODO CHANGE THESE
                achievement.Target = 999;
                achievement.Task = Achievement.TaskType.RedeemItems;

                db.SaveChanges();

                if (isEdit)
                {
                    ShowNotification("Achievement " + achievement.Id + " updated successfully!");
                } else
                {
                    ShowNotification("Achievement " + achievement.Id + " updated successfully!");
                }
                ResetAchForm();
                BindAchievements();
            }
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
                //TODO ADD THESE
                //achievement.Points;
                //achievement.Target;
                //achievement.Task;

                litAchFormTitle.Text = "✏️ Edit Achievement (" + achievement.Id + ")";
                btnAddAch.Text = "💾 Save Achievement Changes";
                btnCancelAchEdit.Visible = true;

                ShowNotification("Achievement " + achievement.Name + " loaded into editor. Make changes and click 'Save Achievement Changes'.");
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

        protected void btnCancelAchEdit_Click(object sender, EventArgs e)
        {
            ResetAchForm();
            ShowNotification("Achievement edit cancelled.");
        }

        private void ResetAchForm()
        {
            hfEditingAchId.Value = "";
            litAchFormTitle.Text = "➕ Create Achievement";
            btnAddAch.Text = "➕ Create Achievement";
            btnCancelAchEdit.Visible = false;

            txtAchTitle.Text = "";
            txtAchIcon.Text = "🏆";
            txtAchXp.Text = "100";
            txtAchDesc.Text = "";
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}