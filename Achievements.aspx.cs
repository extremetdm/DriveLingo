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
            if (!IsPostBack)
            {
                BindAchievements();
            }
        }

        private void BindAchievements()
        {
            var output = AchievementService.GetUserAchievements(CurrentUser?.Id);
            rptAchievements.DataSource = output.Achievements;
            rptAchievements.DataBind();
        }

        protected int CalculatePercentage(int progress, int target)
        {
            return (int) Math.Round((double) progress / target * 100);
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}