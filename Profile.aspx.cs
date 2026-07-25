using DriveLingo.Data;
using DriveLingo.Models;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo
{
    public partial class UserProfilePage : AuthPage
    {
        public class InventoryItemViewModel
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Icon { get; set; }
            public string Category { get; set; }
            public bool IsEquipped { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();

            if (!IsPostBack)
            {
                BindUserProfile();
                BindInventory();
            }
        }

        private User ActiveUser
        {
            get
            {
                var state = AppStateRepository.GetCurrent();
                return Session["CurrentUser"] as User ?? state.Users.FirstOrDefault(u => u.Role == "learner") ?? new User();
            }
        }

        private void BindUserProfile()
        {
            var user = ActiveUser;

            litUserName.Text = user.Name;
            litUserEmail.Text = user.Email;
            litRoleBadge.Text = user.Role.ToUpper();
            litJoinedDate.Text = !string.IsNullOrEmpty(user.JoinedDate) ? user.JoinedDate : "2026-07-01";

            txtName.Text = user.Name;

            // Render current display avatar
            litAvatar.Text = user.DisplayAvatar;

            // Render equipped badge Icon directly behind user name (e.g., Alex Hero 🏆)
            if (!string.IsNullOrEmpty(user.EquippedBadge))
            {
                var badgeItem = AppStateRepository.GetCurrent().StoreItems.FirstOrDefault(i => i.Category == "Badge" && (i.Title == user.EquippedBadge || i.Id == user.EquippedBadge || i.Icon == user.EquippedBadge));
                string badgeIcon = badgeItem != null ? badgeItem.Icon : user.EquippedBadge;

                litUserBadge.Text = " <span title='" + user.EquippedBadge + "' style='font-size: 1.25rem; vertical-align: middle;'>" + badgeIcon + "</span>";
            }
            else
            {
                litUserBadge.Text = "";
            }

            // Apply equipped border glow with Admin's dynamic defined color
            string borderColor = user.EquippedBorderColor;
            if (string.IsNullOrEmpty(borderColor) && !string.IsNullOrEmpty(user.EquippedBorder))
            {
                var borderItem = AppStateRepository.GetCurrent().StoreItems.FirstOrDefault(i => i.Category == "Border" && (i.Title == user.EquippedBorder || i.Id == user.EquippedBorder));
                if (borderItem != null && !string.IsNullOrEmpty(borderItem.ColorHex))
                {
                    borderColor = borderItem.ColorHex;
                }
            }

            if (!string.IsNullOrEmpty(borderColor))
            {
                divAvatarBox.Style["box-shadow"] = "0 0 15px " + borderColor + ", 0 0 25px " + borderColor;
                divAvatarBox.Style["border"] = "2px solid " + borderColor;
            }
            else
            {
                divAvatarBox.Style.Remove("box-shadow");
                divAvatarBox.Style["border"] = "2px solid var(--primary)";
            }
        }

        private void BindInventory()
        {
            var user = ActiveUser;
            var state = AppStateRepository.GetCurrent();

            var list = new List<InventoryItemViewModel>();

            // Always display the Default Learner Icon so candidate can switch back to default car anytime!
            bool isDefaultIconEquipped = string.IsNullOrEmpty(user.EquippedIcon) || user.EquippedIcon == "🚗";
            list.Add(new InventoryItemViewModel
            {
                Id = "default_learner_car",
                Title = "🚗 Default Learner Car",
                Icon = "🚗",
                Category = "Icon",
                IsEquipped = isDefaultIconEquipped
            });

            // Map user owned inventory items from Store
            if (user.Inventory != null)
            {
                foreach (string itemId in user.Inventory)
                {
                    var storeItem = state.StoreItems.FirstOrDefault(i => i.Id == itemId || i.Title == itemId);
                    if (storeItem != null)
                    {
                        bool isEquipped = false;
                        if (storeItem.Category == "Border")
                        {
                            isEquipped = user.EquippedBorder == storeItem.Title || user.EquippedBorder == storeItem.Id;
                        }
                        else if (storeItem.Category == "Icon")
                        {
                            isEquipped = user.EquippedIcon == storeItem.Icon;
                        }
                        else if (storeItem.Category == "Badge")
                        {
                            isEquipped = user.EquippedBadge == storeItem.Title || user.EquippedBadge == storeItem.Id;
                        }

                        list.Add(new InventoryItemViewModel
                        {
                            Id = storeItem.Id,
                            Title = storeItem.Title,
                            Icon = storeItem.Icon,
                            Category = storeItem.Category,
                            IsEquipped = isEquipped
                        });
                    }
                }
            }

            rptInventory.DataSource = list;
            rptInventory.DataBind();
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            var user = ActiveUser;
            if (user.Role == "guest" || (Session["IsGuestMode"] != null && (bool)Session["IsGuestMode"]))
            {
                ShowNotification("🔍 Guest Mode: Please sign in to modify profile settings!");
                return;
            }

            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowNotification("Please enter a valid display name.");
                return;
            }

            user.Name = name;

            string password = txtNewPassword.Text.Trim();
            if (!string.IsNullOrEmpty(password))
            {
                user.Password = password;
            }

            Session["CurrentUser"] = user;
            ShowNotification("Profile settings updated successfully!");

            BindUserProfile();
            if (Master is SiteMaster masterPage)
            {
                masterPage.UpdateUserHeaderAndNavigation();
            }
        }

        protected void rptInventory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var user = ActiveUser;
            string itemId = e.CommandArgument.ToString();
            var state = AppStateRepository.GetCurrent();

            if (e.CommandName == "EquipItem")
            {
                if (itemId == "default_learner_car")
                {
                    user.EquippedIcon = ""; // Switch back to default learner car icon
                    ShowNotification("Equipped default 🚗 Learner Car icon!");
                }
                else
                {
                    var item = state.StoreItems.FirstOrDefault(i => i.Id == itemId || i.Title == itemId);
                    if (item != null)
                    {
                        if (item.Category == "Border")
                        {
                            user.EquippedBorder = item.Title;
                            user.EquippedBorderColor = !string.IsNullOrEmpty(item.ColorHex) ? item.ColorHex : "#6366f1";
                            ShowNotification("Equipped border frame: " + item.Title + "!");
                        }
                        else if (item.Category == "Icon")
                        {
                            user.EquippedIcon = item.Icon;
                            ShowNotification("Equipped custom avatar icon: " + item.Icon + " " + item.Title + "!");
                        }
                        else if (item.Category == "Badge")
                        {
                            user.EquippedBadge = item.Title;
                            ShowNotification("Equipped name badge: " + item.Title + "!");
                        }
                    }
                }
            }
            else if (e.CommandName == "UnequipItem")
            {
                if (itemId == "default_learner_car")
                {
                    // Do nothing for default
                }
                else
                {
                    var item = state.StoreItems.FirstOrDefault(i => i.Id == itemId || i.Title == itemId);
                    if (item != null)
                    {
                        if (item.Category == "Border" && (user.EquippedBorder == item.Title || user.EquippedBorder == item.Id))
                        {
                            user.EquippedBorder = "";
                            user.EquippedBorderColor = "";
                            ShowNotification("Unequipped border frame.");
                        }
                        else if (item.Category == "Icon" && user.EquippedIcon == item.Icon)
                        {
                            user.EquippedIcon = "";
                            ShowNotification("Unequipped custom icon. Switched back to default icon.");
                        }
                        else if (item.Category == "Badge" && (user.EquippedBadge == item.Title || user.EquippedBadge == item.Id))
                        {
                            user.EquippedBadge = "";
                            ShowNotification("Unequipped name badge.");
                        }
                    }
                }
            }

            Session["CurrentUser"] = user;

            BindUserProfile();
            BindInventory();
            if (Master is SiteMaster masterPage)
            {
                masterPage.UpdateUserHeaderAndNavigation();
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = message;
        }
    }
}
