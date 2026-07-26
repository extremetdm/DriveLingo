using DriveLingo.Database;
using DriveLingo.Database.Models;
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
    public partial class UserProfilePage : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();

            if (!IsPostBack)
            {
                BindUserProfile();
                BindInventory();
            }
        }

        struct OwnedItem
        {
            public int Id { get; set; }
            public string Icon { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public bool IsEquipped { get; set; }
        }

        private void BindUserProfile()
        {
            litUserName.Text = CurrentUser.Username;
            litUserEmail.Text = CurrentUser.Email;
            litJoinedDate.Text = CurrentUser.RegisteredAt.ToString();

            string roleSymbol;
            switch (CurrentUser.Role)
            {
                case Database.Models.User.UserRole.Admin:
                    roleSymbol = "👑 ";
                    break;
                case Database.Models.User.UserRole.Instructor:
                    roleSymbol = "👨‍✈️ ";
                    break;
                case Database.Models.User.UserRole.Learner:
                    roleSymbol = "🚘 ";
                    break;
                default:
                    roleSymbol = "";
                    break;
            }
            litRoleBadge.Text = roleSymbol + CurrentUser.Role.ToString().ToUpper();

            txtName.Text = CurrentUser.Username;

            var equippedItems = Context.Items["EquippedItems"] as List<ShopItem>;

            var equippedBorder = equippedItems?.FirstOrDefault(i => i.Type == ShopItem.ItemType.Border);
            var equippedIcon = equippedItems?.FirstOrDefault(i => i.Type == ShopItem.ItemType.Icon);
            var equippedBadge = equippedItems?.FirstOrDefault(i => i.Type == ShopItem.ItemType.Badge);

            // Render current display avatar
            litAvatar.Text = equippedIcon?.Icon ?? CurrentUser.Avatar;

            // Render equipped badge Icon directly behind user name (e.g., Alex Hero 🏆)
            if (equippedBadge != null)
            {
                litUserBadge.Text = " <span title='" + equippedBadge.Name + "' style='font-size: 1.25rem; vertical-align: middle;'>" + equippedBadge.Icon + "</span>";
            }
            else
            {
                litUserBadge.Text = "";
            }

            if (equippedBorder != null)
            {
                var borderColor = equippedBorder.ColorHex;
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
            using (var db = new AppDbContext())
            {
                var items = db.ShopRedemptions
                    .Where(r => r.UserId == CurrentUser.Id)
                    .Select(r => new
                    {
                        r.Id,
                        r.Item.Icon,
                        r.Item.Name,
                        r.Item.Type,
                        r.IsEquiped
                    })
                    .ToList()
                    .Select(i => new OwnedItem
                    {
                        Id = i.Id,
                        Icon = i.Icon,
                        Name = i.Name,
                        Type = i.Type.ToString(),
                        IsEquipped = i.IsEquiped
                    })
                    .ToList();
                //// Always display the Default Learner Icon so candidate can switch back to default car anytime!
                //list.Add(new InventoryItemViewModel
                //{
                //    Id = "default_learner_car",
                //    Title = "🚗 Default Learner Car",
                //    Icon = "🚗",
                //    Category = "Icon",
                //    IsEquipped = isDefaultIconEquipped
                //});

                if (items.Count > 0)
                {
                    rptInventory.DataSource = items;
                    rptInventory.DataBind();
                }
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {

            //string avatar = ddlAvatar.SelectedValue;
            //user.Avatar = avatar;

            string username = txtName.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                ShowNotification("Username cannot be empty.");
                return;
            }

            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(CurrentUser.Id);
                if (user == null)
                {
                    ShowNotification("User not found.");
                    return;
                }

                var sameUsernameUser = db.Users
                    .Where(u => u.Id != CurrentUser.Id && u.Username == username)
                    .FirstOrDefault();
                if (sameUsernameUser != null)
                {
                    ShowNotification("Username already taken.");
                    return;
                }

                user.Username = username;
                string newPassword = txtNewPassword.Text.Trim();
                if (!string.IsNullOrEmpty(newPassword))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                }

                db.SaveChanges();

                AuthService.RefreshCurrentUser(db, user);

                BindUserProfile();
                ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
                ShowNotification("Profile settings updated successfully!");
            }
        }

        protected void rptInventory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EquipItem")
            {
                string invenIdArg = e.CommandArgument.ToString();

                using (var db = new AppDbContext())
                {
                    //if (itemId == "default_learner_car")
                    //{
                    //    user.EquippedIcon = ""; // Switch back to default learner car icon
                    //    ShowNotification("Equipped default 🚗 Learner Car icon!");
                    //}

                    int invenId;
                    if (!int.TryParse(invenIdArg, out invenId))
                    {
                        ShowNotification("Invalid item selected.");
                        return;
                    }

                    var user = db.Users.Find(CurrentUser.Id);
                    if (user == null)
                    {
                        ShowNotification("User not found.");
                        return;
                    }

                    var inven = db.ShopRedemptions.Find(invenId);
                    if (inven == null)
                    {
                        ShowNotification("Invalid item selected.");
                        return;
                    }

                    inven.IsEquiped = true;
                    var itemType = inven.Item.Type;

                    var sameTypeItems = user.ShopRedemptions
                        .Where(r => r.Item.Type == itemType && r.Id != inven.Id)
                        .ToList();

                    foreach (var r in sameTypeItems)
                    {
                        r.IsEquiped = false;
                    }

                    db.SaveChanges();

                    AuthService.RefreshCurrentUser(db, user);

                    switch (itemType)
                    {
                        case ShopItem.ItemType.Icon:
                            ShowNotification("Equipped custom avatar icon: " + inven.Item.Icon + " " + inven.Item.Name + "!");
                            break;
                        case ShopItem.ItemType.Badge:
                            ShowNotification("Equipped name badge: " + inven.Item.Name + "!");
                            break;
                        case ShopItem.ItemType.Border:
                            ShowNotification("Equipped border frame: " + inven.Item.Name + "!");
                            break;
                    }
                    BindUserProfile();
                    BindInventory();
                    if (Master is SiteMaster masterPage)
                    {
                        masterPage.UpdateUserHeaderAndNavigation();
                    }
                }
            }
            else if (e.CommandName == "UnequipItem")
            {
                //if (itemId == "default_learner_car")
                //{
                //    // Do nothing for default
                //}

                string invenIdArg = e.CommandArgument.ToString();

                using (var db = new AppDbContext())
                {
                    //if (itemId == "default_learner_car")
                    //{
                    //    user.EquippedIcon = ""; // Switch back to default learner car icon
                    //    ShowNotification("Equipped default 🚗 Learner Car icon!");
                    //}

                    int invenId;
                    if (!int.TryParse(invenIdArg, out invenId))
                    {
                        ShowNotification("Invalid item selected.");
                        return;
                    }

                    var user = db.Users.Find(CurrentUser.Id);
                    if (user == null)
                    {
                        ShowNotification("User not found.");
                        return;
                    }

                    var inven = db.ShopRedemptions.Find(invenId);
                    if (inven == null)
                    {
                        ShowNotification("Invalid item selected.");
                        return;
                    }

                    inven.IsEquiped = false;
                    db.SaveChanges();
                    AuthService.RefreshCurrentUser(db, user);

                    switch (inven.Item.Type)
                    {
                        case ShopItem.ItemType.Icon:
                            ShowNotification("Unequipped custom icon. Switched back to default icon.");
                            break;
                        case ShopItem.ItemType.Badge:
                            ShowNotification("Unequipped name badge.");
                            break;
                        case ShopItem.ItemType.Border:
                            ShowNotification("Unequipped border frame.");
                            break;
                    }
                    BindUserProfile();
                    BindInventory();
                    if (Master is SiteMaster masterPage)
                    {
                        masterPage.UpdateUserHeaderAndNavigation();
                    }
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
