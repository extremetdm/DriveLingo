using System;
using System.Web;
using System.Web.UI;
using DriveLingo.Database.Models;

namespace DriveLingo
{
    using Services;
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
            var currentUser = Context.Items["CurrentUser"] as User;
            string currentPage = Page.AppRelativeVirtualPath.ToLower();

            bool isAuthPage = currentPage.EndsWith("login.aspx") || currentPage.EndsWith("register.aspx");

            if (currentUser != null && !isAuthPage)
            {
                divAppContainer.Attributes["class"] = "app-layout";
                appSidebar.Visible = true;
                topHeader.Visible = true;
                phUserFooter.Visible = true;
                phGuestFooter.Visible = false;

                string defaultAvatar;
                string roleSymbol;
                switch (currentUser.Role)
                {
                    case User.UserRole.Admin:
                        defaultAvatar = "👑";
                        roleSymbol = "👑 ";
                        break;
                    case User.UserRole.Instructor:
                        defaultAvatar = "👨‍✈️";
                        roleSymbol = "👨‍✈️ ";
                        break;
                    case User.UserRole.Learner:
                        defaultAvatar = "🚗";
                        roleSymbol = "🚘 ";
                        break;
                    default:
                        defaultAvatar = "";
                        roleSymbol = "";
                        break;
                }

                litAvatar.Text = defaultAvatar;
                //litAvatar.Text = string.IsNullOrEmpty(currentUser.Avatar) ? defaultAvatar : currentUser.Avatar;
                litUserName.Text = currentUser.Username;
                litUserRole.Text = "Role: " + roleSymbol + currentUser.Role.ToString().ToUpper();

                // Apply equipped cosmetic border glow if any
                
                //if (!string.IsNullOrEmpty(currentUser.EquippedBorder) && currentUser.EquippedBorder.Contains("Glowing Neon"))
                //{
                //    divSidebarAvatar.Style["box-shadow"] = "0 0 12px var(--primary), 0 0 24px var(--secondary)";
                //    divSidebarAvatar.Style["border"] = "2px solid var(--primary)";
                //}
                //else
                //{
                    divSidebarAvatar.Style.Remove("box-shadow");
                    divSidebarAvatar.Style.Remove("border");
                //}

                // Show badges & level/XP progress strictly for Candidate / Learner
                if (currentUser.Role == User.UserRole.Learner)
                {
                    phLearnerBadges.Visible = true;
                    litHeaderLevel.Text = currentUser.CurrentLevel.ToString();
                    litHeaderPoints.Text = currentUser.Points.ToString();

                    int percent = (int)Math.Round((double)currentUser.XpProgress / currentUser.NextLevelXpRequired * 100);
                    litHeaderXpText.Text = currentUser.XpProgress + " / " + currentUser.NextLevelXpRequired + " XP";
                    divHeaderXpBar.Style["width"] = percent + "%";
                }
                else
                {
                    phLearnerBadges.Visible = false;
                }

                // Role based navigation links - strictly show role specific sidebar section
                phLearnerNav.Visible = (currentUser.Role == User.UserRole.Learner);
                phEducatorNav.Visible = (currentUser.Role == User.UserRole.Instructor);
                phAdminNav.Visible = (currentUser.Role == User.UserRole.Admin);
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
            AuthService.Logout(Context);
            Response.Redirect("~/Login.aspx");
        }
    }
}
