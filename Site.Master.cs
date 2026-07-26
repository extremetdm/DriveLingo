using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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
            var currentUser = Context.Items["CurrentUser"] as User;
            string currentPage = Page.AppRelativeVirtualPath.ToLower();

            bool isLandingPage = currentPage.Equals("~/default.aspx", StringComparison.OrdinalIgnoreCase) || currentPage.Equals("~/", StringComparison.OrdinalIgnoreCase) || currentPage.Equals("~/default", StringComparison.OrdinalIgnoreCase);
            bool isAuthPage = currentPage.EndsWith("login.aspx") || currentPage.EndsWith("register.aspx");
            bool hideSidebar = isLandingPage || isAuthPage;

            bool isGuestMode = currentUser == null || currentUser.Role == User.UserRole.Guest;
            pnlGuestBanner.Visible = isGuestMode && !hideSidebar;

            if (!hideSidebar)
            {
                divAppContainer.Attributes["class"] = "app-layout";
                appSidebar.Visible = true;
                topHeader.Visible = true;
                phUserFooter.Visible = !isGuestMode;
                phGuestFooter.Visible = isGuestMode;

                var role = isGuestMode? User.UserRole.Guest: currentUser.Role;
                string username = isGuestMode? "Guest Candidate": currentUser.Username;
                int points = currentUser?.Points ?? 0;
                int level = currentUser?.CurrentLevel ?? 0;
                int xpProgress = currentUser?.XpProgress ?? 0;
                int xpRequired = currentUser?.NextLevelXpRequired ?? LevelingService.CalculateRequiredXP(1);

                var equippedItems = Context.Items["EquippedItems"] as List<ShopItem>;

                var equippedBorder = equippedItems?.FirstOrDefault(i => i.Type == ShopItem.ItemType.Border);
                var equippedIcon = equippedItems?.FirstOrDefault(i => i.Type == ShopItem.ItemType.Icon);
                var equippedBadge = equippedItems?.FirstOrDefault(i => i.Type == ShopItem.ItemType.Badge);

                string roleSymbol;
                switch (role)
                {
                    case User.UserRole.Admin:
                        roleSymbol = "👑 ";
                        break;
                    case User.UserRole.Instructor:
                        roleSymbol = "👨‍✈️ ";
                        break;
                    case User.UserRole.Guest:
                        roleSymbol = "🔍 ";
                        break;
                    default:
                        roleSymbol = "🚘 ";
                        break;
                }

                string avatarIcon = equippedIcon?.Icon ?? currentUser?.Avatar ?? "🚗";

                litAvatar.Text = avatarIcon;
                litUserName.Text = username;
                litUserRole.Text = "Role: " + roleSymbol + role.ToString().ToUpper();

                // Display Equipped Badge Icon directly behind username (e.g. "Alex Hero 🏆")
                if (equippedBadge != null)
                {
                    litUserEquippedBadge.Text = " <span title='" + equippedBadge.Name + "' style='font-size: 1.1rem; vertical-align: middle;'>" + equippedBadge.Icon + "</span>";
                }
                else
                {
                    litUserEquippedBadge.Text = "";
                }

                // Apply equipped cosmetic border glow frame with Admin's defined color

                if (equippedBorder != null)
                {
                    var borderColor = equippedBorder.ColorHex;
                    divSidebarAvatar.Style["box-shadow"] = "0 0 15px " + borderColor + ", 0 0 25px " + borderColor;
                    divSidebarAvatar.Style["border"] = "2px solid " + borderColor;
                }
                else
                {
                    divSidebarAvatar.Style.Remove("box-shadow");
                    divSidebarAvatar.Style.Remove("border");
                }

                // Show learner XP stats
                if (role == User.UserRole.Learner || role == User.UserRole.Guest)
                {
                    phLearnerBadges.Visible = true;
                    litHeaderLevel.Text = level.ToString();
                    litHeaderPoints.Text = points.ToString();

                    int percent = (int)Math.Round((double)xpProgress / xpRequired * 100);
                    if (percent > 100) percent = 100;
                    litHeaderXpText.Text = xpProgress + " / " + xpRequired + " XP";
                    divHeaderXpBar.Style["width"] = percent + "%";
                }
                else
                {
                    phLearnerBadges.Visible = false;
                }

                // Navigation Visibility
                phLearnerNav.Visible = role == User.UserRole.Guest || role == User.UserRole.Learner;
                phEducatorNav.Visible = role == User.UserRole.Instructor;
                phAdminNav.Visible = role == User.UserRole.Admin;
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
            AuthService.Logout(Context);
            Response.Redirect("~/Login.aspx");
        }
    }
}
