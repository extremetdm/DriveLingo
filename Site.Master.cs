using System;
using System.Web;
using System.Web.UI;
using DriveLingo.Data;
using DriveLingo.Models;

namespace DriveLingo
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateUserHeaderAndNavigation();
            }
        }

        public void UpdateUserHeaderAndNavigation()
        {
            User currentUser = Session["CurrentUser"] as User;
            string currentPage = Page.AppRelativeVirtualPath.ToLower();

            bool isAuthPage = currentPage.EndsWith("login.aspx") || currentPage.EndsWith("register.aspx");

            if (currentUser != null && !isAuthPage)
            {
                divAppContainer.Attributes["class"] = "app-layout";
                appSidebar.Visible = true;
                topHeader.Visible = true;
                phUserFooter.Visible = true;
                phGuestFooter.Visible = false;

                litAvatar.Text = string.IsNullOrEmpty(currentUser.Avatar) ? "🚗" : currentUser.Avatar;
                litUserName.Text = currentUser.Name;
                litUserRole.Text = "Role: " + currentUser.Role.ToUpper();

                // Apply equipped cosmetic border glow if any
                if (!string.IsNullOrEmpty(currentUser.EquippedBorder) && currentUser.EquippedBorder.Contains("Glowing Neon"))
                {
                    divSidebarAvatar.Style["box-shadow"] = "0 0 12px var(--primary), 0 0 24px var(--secondary)";
                    divSidebarAvatar.Style["border"] = "2px solid var(--primary)";
                }
                else
                {
                    divSidebarAvatar.Style.Remove("box-shadow");
                    divSidebarAvatar.Style.Remove("border");
                }

                // Show badges & level/XP progress strictly for Candidate / Learner
                if (currentUser.Role == "learner")
                {
                    phLearnerBadges.Visible = true;
                    litHeaderLevel.Text = currentUser.Level.ToString();
                    litHeaderPoints.Text = currentUser.Points.ToString();

                    int currentLevelXp = currentUser.XP % 200;
                    int targetLevelXp = 200;
                    int percent = (int)Math.Round((double)currentLevelXp / targetLevelXp * 100);
                    litHeaderXpText.Text = currentLevelXp + " / " + targetLevelXp + " XP";
                    divHeaderXpBar.Style["width"] = percent + "%";
                }
                else
                {
                    phLearnerBadges.Visible = false;
                }

                // Role based navigation links - strictly show role specific sidebar section
                phLearnerNav.Visible = (currentUser.Role == "learner");
                phEducatorNav.Visible = (currentUser.Role == "educator");
                phAdminNav.Visible = (currentUser.Role == "admin");
            }
            else
            {
                // Hide sidebar completely on Login / Register or when not logged in
                divAppContainer.Attributes["class"] = "app-layout no-sidebar";
                appSidebar.Visible = false;
                topHeader.Visible = !isAuthPage;
                phUserFooter.Visible = false;
                phGuestFooter.Visible = true;
                phLearnerBadges.Visible = false;

                phLearnerNav.Visible = false;
                phEducatorNav.Visible = false;
                phAdminNav.Visible = false;
            }
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            Session["CurrentUser"] = null;
            Response.Redirect("~/Login.aspx");
        }
    }
}
