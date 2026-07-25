using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using DriveLingo.Data;
using DriveLingo.Database.Models;
using DriveLingo.Services;

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
            var dbUser = Context.Items["CurrentUser"] as User;
            var sessionUser = Session["CurrentUser"] as DriveLingo.Models.User;
            string currentPage = Page.AppRelativeVirtualPath.ToLower();

            bool isLandingPage = currentPage.Equals("~/default.aspx", StringComparison.OrdinalIgnoreCase) || currentPage.Equals("~/", StringComparison.OrdinalIgnoreCase) || currentPage.Equals("~/default", StringComparison.OrdinalIgnoreCase);
            bool isAuthPage = currentPage.EndsWith("login.aspx") || currentPage.EndsWith("register.aspx");
            bool hideSidebar = isLandingPage || isAuthPage;
            bool isGuestMode = (Session["IsGuestMode"] != null && (bool)Session["IsGuestMode"]) || (sessionUser != null && sessionUser.Role == "guest");

            pnlGuestBanner.Visible = isGuestMode && !hideSidebar;

            if ((dbUser != null || sessionUser != null) && !hideSidebar)
            {
                divAppContainer.Attributes["class"] = "app-layout";
                appSidebar.Visible = true;
                topHeader.Visible = true;
                phUserFooter.Visible = true;
                phGuestFooter.Visible = false;

                string roleStr = dbUser != null ? dbUser.Role.ToString().ToLower() : (sessionUser != null ? sessionUser.Role.ToLower() : "learner");
                string username = dbUser != null ? dbUser.Username : (sessionUser != null ? sessionUser.Name : "Guest Candidate");
                int points = dbUser != null ? dbUser.Points : (sessionUser != null ? sessionUser.Points : 0);
                int level = dbUser != null ? dbUser.CurrentLevel : (sessionUser != null ? sessionUser.Level : 1);
                int xpProgress = dbUser != null ? dbUser.XpProgress : (sessionUser != null ? sessionUser.XP : 0);
                int xpRequired = dbUser != null ? dbUser.NextLevelXpRequired : 200;

                string equippedBorder = sessionUser != null ? sessionUser.EquippedBorder : "";
                string equippedIcon = sessionUser != null ? sessionUser.EquippedIcon : "";
                string equippedBadge = sessionUser != null ? sessionUser.EquippedBadge : "";

                string roleSymbol;
                switch (roleStr)
                {
                    case "admin":
                        roleSymbol = "👑 ";
                        break;
                    case "instructor":
                    case "educator":
                        roleSymbol = "👨‍✈️ ";
                        break;
                    case "guest":
                        roleSymbol = "🔍 ";
                        break;
                    default:
                        roleSymbol = "🚘 ";
                        break;
                }

                // Avatar Display: Equipped custom icon or fixed role default
                string avatarIcon = "🚗";
                if (!string.IsNullOrEmpty(equippedIcon))
                {
                    avatarIcon = equippedIcon;
                }
                else if (roleStr == "admin")
                {
                    avatarIcon = "👑";
                }
                else if (roleStr == "instructor" || roleStr == "educator")
                {
                    avatarIcon = "👨‍✈️";
                }
                else
                {
                    avatarIcon = "🚗"; // Default Learner Icon
                }

                litAvatar.Text = avatarIcon;
                litUserName.Text = username;
                litUserRole.Text = "Role: " + roleSymbol + roleStr.ToUpper();

                // Display Equipped Badge Icon directly behind username (e.g. "Alex Hero 🏆")
                if (!string.IsNullOrEmpty(equippedBadge))
                {
                    var badgeItem = AppStateRepository.GetCurrent().StoreItems.FirstOrDefault(i => i.Category == "Badge" && (i.Title == equippedBadge || i.Id == equippedBadge || i.Icon == equippedBadge));
                    string badgeIcon = badgeItem != null ? badgeItem.Icon : equippedBadge;

                    litUserEquippedBadge.Text = " <span title='" + equippedBadge + "' style='font-size: 1.1rem; vertical-align: middle;'>" + badgeIcon + "</span>";
                }
                else
                {
                    litUserEquippedBadge.Text = "";
                }

                // Apply equipped cosmetic border glow frame with Admin's defined color
                string borderColor = sessionUser != null ? sessionUser.EquippedBorderColor : "";
                if (string.IsNullOrEmpty(borderColor) && !string.IsNullOrEmpty(equippedBorder))
                {
                    var borderItem = AppStateRepository.GetCurrent().StoreItems.FirstOrDefault(i => i.Category == "Border" && (i.Title == equippedBorder || i.Id == equippedBorder));
                    if (borderItem != null && !string.IsNullOrEmpty(borderItem.ColorHex))
                    {
                        borderColor = borderItem.ColorHex;
                    }
                }

                if (!string.IsNullOrEmpty(borderColor))
                {
                    divSidebarAvatar.Style["box-shadow"] = "0 0 15px " + borderColor + ", 0 0 25px " + borderColor;
                    divSidebarAvatar.Style["border"] = "2px solid " + borderColor;
                }
                else
                {
                    divSidebarAvatar.Style.Remove("box-shadow");
                    divSidebarAvatar.Style.Remove("border");
                }

                // Show learner XP stats
                if (roleStr == "learner" || roleStr == "guest")
                {
                    phLearnerBadges.Visible = true;
                    litHeaderLevel.Text = level.ToString();
                    litHeaderPoints.Text = points.ToString();

                    int percent = (int)Math.Round((double)xpProgress / (xpRequired > 0 ? xpRequired : 200) * 100);
                    if (percent > 100) percent = 100;
                    litHeaderXpText.Text = xpProgress + " / " + xpRequired + " XP";
                    divHeaderXpBar.Style["width"] = percent + "%";
                }
                else
                {
                    phLearnerBadges.Visible = false;
                }

                // Navigation Visibility
                phLearnerNav.Visible = (roleStr == "learner" || roleStr == "guest");
                phEducatorNav.Visible = (roleStr == "instructor" || roleStr == "educator");
                phAdminNav.Visible = (roleStr == "admin");
            }
            else
            {
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
            Session.Clear();
            AuthService.Logout(Context);
            Response.Redirect("~/Login.aspx");
        }
    }
}
