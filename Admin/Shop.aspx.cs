using DriveLingo.Database;
using DriveLingo.Database.Models;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo.Admin
{
    public partial class Shop : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Admin);

            if (!IsPostBack)
            {
                BindStore();
            }
        }

        private void BindStore()
        {
            using (var db = new AppDbContext())
            {
                gvStore.DataSource = db.ShopItems.ToList();
                gvStore.DataBind();
            }
        }

        protected void btnAddStoreItem_Click(object sender, EventArgs e)
        {
            string name = txtStoreTitle.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowNotification("Please enter item title.");
                return;
            }

            ShopItem.ItemType type;
            if (!Enum.TryParse(ddlCategory.SelectedValue, out type)) {
                ShowNotification("Please select item type.");
                return;
            }

            int price;
            if (!int.TryParse(txtStorePrice.Text.Trim(), out price))
            {
                ShowNotification("Please enter a valid price.");
                return;
            }

            string icon = txtStoreIcon.Text.Trim(); // TODO CHANGE TO IMAGE OR SMTH WTF WHO USES EMOJI ICONS
            if (string.IsNullOrEmpty(icon))
            {
                ShowNotification("Please enter icon.");
                return;
            }

            string colorHex = txtColorHex.Text.Trim();

            string description = txtStoreDesc.Text.Trim();
            if (string.IsNullOrEmpty(description))
            {
                ShowNotification("Please enter description.");
                return;
            }

            using (var db = new AppDbContext())
            {
                string itemId = hfEditingStoreItemId.Value;
                ShopItem item = null;

                if (!string.IsNullOrEmpty(itemId))
                {
                    item = db.ShopItems.Find(Convert.ToInt32(itemId));
                }

                bool isEdit = item != null;

                if (!isEdit)
                {
                    item = new ShopItem();
                    db.ShopItems.Add(item);
                }

                item.Name = name;
                item.Type = type;
                item.Icon = string.IsNullOrEmpty(icon) ? "✨" : icon;
                item.Description = description;
                item.Cost = price;
                item.ColorHex = colorHex;

                db.SaveChanges();

                if (isEdit)
                    ShowNotification("Store item " + name + " updated successfully!");
                else
                    ShowNotification("New store item added!");

                ResetStoreForm();
                BindStore();


            }
        }

        protected void btnCancelStoreEdit_Click(object sender, EventArgs e)
        {
            ResetStoreForm();
        }

        private void ResetStoreForm()
        {
            hfEditingStoreItemId.Value = "";
            txtStoreTitle.Text = "";
            txtStoreIcon.Text = "✨";
            txtStorePrice.Text = "200";
            txtColorHex.Text = "#6366f1";
            txtStoreDesc.Text = "";
            ddlCategory.SelectedValue = "Border";
            litStoreFormTitle.Text = "➕ Create Store Item";
            btnAddStoreItem.Text = "➕ Create Store Item";
            btnCancelStoreEdit.Visible = false;
        }

        protected void gvStore_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditStoreItem")
            {
                handleEdit(sender, e);
            }
            else if (e.CommandName == "DeleteStoreItem")
            {
                handleDelete(sender, e);
            }
        }

        protected void handleEdit(object sender, GridViewCommandEventArgs e)
        {
            int itemId = Convert.ToInt32(e.CommandArgument.ToString());
            using (var db = new AppDbContext())
            {
                var item = db.ShopItems.Find(itemId);
                if (item == null)
                {
                    ShowNotification("Store item not found.");
                    return;
                }

                hfEditingStoreItemId.Value = item.Id.ToString();
                txtStoreTitle.Text = item.Name;
                txtStoreIcon.Text = item.Icon;
                txtStorePrice.Text = item.Cost.ToString();
                txtStoreDesc.Text = item.Description;
                txtColorHex.Text = item.ColorHex;
                if (ddlCategory.Items.FindByValue(item.Type.ToString()) != null)
                {
                    ddlCategory.SelectedValue = item.Type.ToString();
                }

                litStoreFormTitle.Text = "✏️ Edit Store Item";
                btnAddStoreItem.Text = "💾 Save Changes";
                btnCancelStoreEdit.Visible = true;
            }
        }

        protected void handleDelete(object sender, GridViewCommandEventArgs e)
        {
            int itemId = Convert.ToInt32(e.CommandArgument.ToString());
            using (var db = new AppDbContext())
            {
                var item = db.ShopItems.Find(itemId);
                if (item == null)
                {
                    ShowNotification("Store item not found.");
                    return;
                }
                db.ShopItems.Remove(item);

                db.SaveChanges();
                ShowNotification($"Store item '{item.Name}' deleted.");
                ResetStoreForm();
                BindStore();
            }
        }


        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = message;
        }
    }
}