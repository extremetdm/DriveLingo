using System;
using System.Web.UI;
using DriveLingo.Data;
using DriveLingo.Models;

namespace DriveLingo
{
    public partial class UserProfilePage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User user = Session["CurrentUser"] as User;
            if (user == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProfile(user);
            }
        }

        private void LoadProfile(User user)
        {
            litAvatar.Text = string.IsNullOrEmpty(user.Avatar) ? "🚗" : user.Avatar;
            litRoleBadge.Text = user.Role.ToUpper();
            litUserName.Text = user.Name;
            litUserEmail.Text = user.Email;
            litJoinedDate.Text = user.JoinedDate;

            txtName.Text = user.Name;
            if (ddlAvatar.Items.FindByValue(user.Avatar) != null)
            {
                ddlAvatar.SelectedValue = user.Avatar;
            }

            if (user.Inventory.Count > 0)
            {
                rptInventory.DataSource = user.Inventory;
                rptInventory.DataBind();
                lblNoInventory.Visible = false;
            }
            else
            {
                lblNoInventory.Visible = true;
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            User user = Session["CurrentUser"] as User;
            if (user == null) return;

            string name = txtName.Text.Trim();
            string avatar = ddlAvatar.SelectedValue;
            string newPassword = txtNewPassword.Text.Trim();

            if (!string.IsNullOrEmpty(name))
            {
                user.Name = name;
            }

            user.Avatar = avatar;

            if (!string.IsNullOrEmpty(newPassword))
            {
                user.Password = newPassword;
            }

            LoadProfile(user);
            ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
            ShowNotification("Profile preferences updated successfully!");
        }

        protected void rptInventory_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                string item = (string)e.Item.DataItem;
                User currentUser = Session["CurrentUser"] as User;
                System.Web.UI.WebControls.Button btnEquipItem = (System.Web.UI.WebControls.Button)e.Item.FindControl("btnEquipItem");

                if (item != null && currentUser != null && btnEquipItem != null)
                {
                    bool isEquipped = (currentUser.EquippedBorder == item);
                    btnEquipItem.Text = isEquipped ? "Equipped ✔" : "Equip Item";
                    btnEquipItem.CssClass = isEquipped ? "btn btn-secondary btn-sm" : "btn btn-primary btn-sm";
                }
            }
        }

        protected void rptInventory_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EquipItem")
            {
                string itemName = e.CommandArgument.ToString();
                User user = Session["CurrentUser"] as User;

                if (user != null)
                {
                    if (user.EquippedBorder == itemName)
                    {
                        user.EquippedBorder = "";
                        ShowNotification("Unequipped: " + itemName);
                    }
                    else
                    {
                        user.EquippedBorder = itemName;
                        ShowNotification("Equipped: " + itemName + "!");
                    }

                    LoadProfile(user);
                    ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
                }
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}
