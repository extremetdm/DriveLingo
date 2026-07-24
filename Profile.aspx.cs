using DriveLingo.Database;
using DriveLingo.Database.Models;
using DriveLingo.Models;
using DriveLingo.UI;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Xml.Linq;

namespace DriveLingo
{
    public partial class UserProfilePage : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        struct OwnedItem
        {
            public int Id {  get; set; }
            public string Icon { get; set; }
            public string Name { get; set; }
        }

        private void LoadProfile()
        {
            litAvatar.Text = CurrentUser.Avatar;
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
            litUserName.Text = CurrentUser.Username;
            litUserEmail.Text = CurrentUser.Email;
            litJoinedDate.Text = CurrentUser.RegisteredAt.ToString();

            txtName.Text = CurrentUser.Username;
            if (ddlAvatar.Items.FindByValue(CurrentUser.Avatar) != null)
            {
                ddlAvatar.SelectedValue = CurrentUser.Avatar;
            }

            using (var db = new AppDbContext())
            {
                var items = db.ShopRedemptions
                    .Where(r => r.UserId == CurrentUser.Id)
                    .Select(r => new
                    {
                        r.Id,
                        r.Item.Icon,
                        r.Item.Name
                    })
                    .ToList()
                    .Select(i => new OwnedItem
                    {
                        Id = i.Id,
                        Icon = i.Icon,
                        Name = i.Name,
                    })
                    .ToList();
                if (items.Count > 0)
                {
                    rptInventory.DataSource = items;
                    rptInventory.DataBind();
                    lblNoInventory.Visible = false;
                }
                else
                {
                    lblNoInventory.Visible = true;
                }
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {

            //string avatar = ddlAvatar.SelectedValue;
            //user.Avatar = avatar;

            //TODO ALLOW EDIT EMAIL & REMOVE AVATAR

            string username = txtName.Text.Trim();
            if (!string.IsNullOrEmpty(username))
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
                    .Where(u => u.Id != CurrentUser.Id &&  u.Username == username)
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
                LoadProfile();
                ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
                ShowNotification("Profile preferences updated successfully!");
            }
        }

        protected void rptInventory_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != System.Web.UI.WebControls.ListItemType.Item
                && e.Item.ItemType != System.Web.UI.WebControls.ListItemType.AlternatingItem
            ) return;
            
            var item = (OwnedItem)e.Item.DataItem;

            var btnEquipItem = (System.Web.UI.WebControls.Button)e.Item.FindControl("btnEquipItem");

            if (btnEquipItem != null)
            {
                //bool isEquipped = (currentUser.EquippedBorder == item);
                bool isEquipped = false;

                btnEquipItem.Text = isEquipped ? "Equipped ✔" : "Equip Item";
                btnEquipItem.CssClass = isEquipped ? "btn btn-secondary btn-sm" : "btn btn-primary btn-sm";
            }
        }

        protected void rptInventory_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EquipItem")
            {
                //TODO IMPLEMENT THIS SOMEHOW
                string itemId = e.CommandArgument.ToString();

                //if (user.EquippedBorder == itemName)
                //{
                //    user.EquippedBorder = "";
                //    ShowNotification("Unequipped: " + itemName);
                //}
                //else
                //{
                //    user.EquippedBorder = itemName;
                //    ShowNotification("Equipped: " + itemName + "!");
                //}

                LoadProfile();
                ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}
