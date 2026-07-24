using DriveLingo.Data;
using DriveLingo.Database;
using DriveLingo.Database.Models;
using DriveLingo.Models;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        // --- Store CRUD Handlers ---
        protected void btnAddStoreItem_Click(object sender, EventArgs e)
        {
            string name = txtStoreTitle.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowNotification("Please enter item title.");
                return;
            }

            int price;
            if (!int.TryParse(txtStorePrice.Text.Trim(), out price))
            {
                ShowNotification("Please enter a valid price.");
                return;
            }

            string description = txtStoreDesc.Text.Trim();
            
            string icon = txtStoreIcon.Text.Trim(); // TODO CHANGE TO IMAGE OR SMTH WTF WHO USES EMOJI ICONS
            icon = string.IsNullOrEmpty(icon) ? "✨" : icon;

            using (var db = new AppDbContext())
            {
                string itemId = hfEditingStoreItemId.Value;
                ShopItem item = null;
                
                if (!string.IsNullOrEmpty(itemId))
                {
                    item = db.ShopItems.Find(itemId);
                }

                bool isEdit = item != null;

                if (!isEdit)
                {
                    item = new ShopItem();
                    db.ShopItems.Add(item);
                }

                item.Name = name;
                item.Icon = string.IsNullOrEmpty(icon) ? "✨" : icon;
                item.Description = description;
                item.Cost = price;

                db.SaveChanges();

                if (isEdit)
                    ShowNotification("Store item " + item.Id + " updated successfully!");
                else
                    ShowNotification("New store item added!");

                ResetStoreForm();
                BindStore();


            }
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

                litStoreFormTitle.Text = "✏️ Edit Store Item (" + item.Id + ")";
                btnAddStoreItem.Text = "💾 Save Store Item Changes";
                btnCancelStoreEdit.Visible = true;

                ShowNotification("Store item " + item.Name + " loaded into editor. Make changes and click 'Save Store Item Changes'.");
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
                ShowNotification("Store item deleted.");
                BindStore();
            }
        }

        protected void btnCancelStoreEdit_Click(object sender, EventArgs e)
        {
            ResetStoreForm();
            ShowNotification("Store item edit cancelled.");
        }

        private void ResetStoreForm()
        {
            hfEditingStoreItemId.Value = "";
            litStoreFormTitle.Text = "➕ Create Store Item";
            btnAddStoreItem.Text = "➕ Create Store Item";
            btnCancelStoreEdit.Visible = false;

            txtStoreTitle.Text = "";
            txtStoreIcon.Text = "✨";
            txtStorePrice.Text = "200";
            txtStoreDesc.Text = "";
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}