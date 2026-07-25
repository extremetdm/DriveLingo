using DriveLingo.Data;
using DriveLingo.Database;
using DriveLingo.Models;
using DriveLingo.Services;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo
{
    public partial class Achievements : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();

            if (!IsPostBack)
            {
                BindAchievements();
            }
        }

        private void BindAchievements()
        {
            var output = AchievementService.GetUserAchievements(CurrentUser.Id);
            rptAchievements.DataSource = output.Achievements;
            rptAchievements.DataBind();
        }

        protected void rptAchievements_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var achievement = (AchievementService.AchievementStatus)e.Item.DataItem;
                
                var unlockedPh = (PlaceHolder)e.Item.FindControl("phAchievementUnlocked");
                var lockedPh = (PlaceHolder)e.Item.FindControl("phAchievementLocked");

                bool isUnlocked = achievement.CompletedAt != null;

                unlockedPh.Visible = isUnlocked;
                lockedPh.Visible = !isUnlocked;
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
            // todo add error msg notification
        }
    }
}