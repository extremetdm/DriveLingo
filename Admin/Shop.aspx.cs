using DriveLingo.Data;
using DriveLingo.Models;
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
            var state = AppStateRepository.GetCurrent();
            gvStore.DataSource = state.StoreItems;
            gvStore.DataBind();
        }

        protected void btnAddStoreItem_Click(object sender, EventArgs e)
        {
            string title = txtStoreTitle.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                ShowNotification("Please provide item title.");
                return;
            }

            string category = ddlCategory.SelectedValue;
            string icon = txtStoreIcon.Text.Trim();
            if (string.IsNullOrEmpty(icon)) icon = "✨";

            int price = 200;
            int.TryParse(txtStorePrice.Text.Trim(), out price);

            string colorHex = txtColorHex.Text.Trim();
            if (string.IsNullOrEmpty(colorHex)) colorHex = "#6366f1";

            string description = txtStoreDesc.Text.Trim();

            var state = AppStateRepository.GetCurrent();
            string editingId = hfEditingStoreItemId.Value;

            if (!string.IsNullOrEmpty(editingId))
            {
                var itemToEdit = state.StoreItems.FirstOrDefault(i => i.Id == editingId);
                if (itemToEdit != null)
                {
                    itemToEdit.Title = title;
                    itemToEdit.Category = category;
                    itemToEdit.Icon = icon;
                    itemToEdit.Price = price;
                    itemToEdit.ColorHex = colorHex;
                    itemToEdit.Description = description;

                    ShowNotification("Store item '" + title + "' updated successfully!");
                }
            }
            else
            {
                var newItem = new StoreItem
                {
                    Id = "item_" + Guid.NewGuid().ToString().Substring(0, 8),
                    Title = title,
                    Category = category,
                    Icon = icon,
                    Price = price,
                    ColorHex = colorHex,
                    Description = description
                };
                state.StoreItems.Add(newItem);
                ShowNotification("New store item '" + title + "' (" + category + ") added to catalog!");
            }

            ResetStoreForm();
            BindStore();
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
            string itemId = e.CommandArgument.ToString();
            var state = AppStateRepository.GetCurrent();
            var item = state.StoreItems.FirstOrDefault(i => i.Id == itemId);

            if (e.CommandName == "EditStoreItem")
            {
                if (item != null)
                {
                    hfEditingStoreItemId.Value = item.Id;
                    txtStoreTitle.Text = item.Title;
                    txtStoreIcon.Text = item.Icon;
                    txtStorePrice.Text = item.Price.ToString();
                    txtColorHex.Text = !string.IsNullOrEmpty(item.ColorHex) ? item.ColorHex : "#6366f1";
                    txtStoreDesc.Text = item.Description;
                    if (ddlCategory.Items.FindByValue(item.Category) != null)
                    {
                        ddlCategory.SelectedValue = item.Category;
                    }

                    litStoreFormTitle.Text = "✏️ Edit Store Item";
                    btnAddStoreItem.Text = "💾 Save Changes";
                    btnCancelStoreEdit.Visible = true;
                }
            }
            else if (e.CommandName == "DeleteStoreItem")
            {
                if (item != null)
                {
                    state.StoreItems.Remove(item);
                    ShowNotification("Store item '" + item.Title + "' deleted.");
                    ResetStoreForm();
                    BindStore();
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