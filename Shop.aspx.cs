using DriveLingo.Data;
using DriveLingo.Database;
using DriveLingo.Database.Models;
using DriveLingo.Models;
using DriveLingo.Services;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo
{
    public partial class Shop : AuthPage
    {
        struct ShopAvailableItem
        {
            public int Id { get; set; }
            public string Icon { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
            public int Cost { get; set; }
            public bool Owned { get; set; }

        }


        private string ActiveCategory
        {
            get => ViewState["ActiveCategory"] as string ?? "ALL";
            set => ViewState["ActiveCategory"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStore();
            }
        }

        private void BindStore()
        {
            using (var db = new AppDbContext())
            {
                var query = db.ShopItems
                    .Include(i => i.Redemptions);

                if (ActiveCategory != "ALL")
                {
                    ShopItem.ItemType type;
                    if (Enum.TryParse(ActiveCategory, out type))
                    {
                        query = query.Where(i => i.Type == type);
                    }
                }

                rptStore.DataSource = query
                    .ToList()
                    .Select(i => new ShopAvailableItem
                    {
                        Id = i.Id,
                        Icon = i.Icon,
                        Name = i.Name,
                        Description = i.Description,
                        Cost = i.Cost,
                        Type = i.Type.ToString(),
                        Owned = i.Redemptions.Any(r => r.UserId == CurrentUser?.Id)
                    })
                    .ToList();
                rptStore.DataBind();
            }
        }

        protected void btnCategoryFilter_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            ActiveCategory = btn.CommandArgument;

            btnTabAll.CssClass = "shop-category-btn" + (ActiveCategory == "ALL" ? " active" : "");
            btnTabBorder.CssClass = "shop-category-btn" + (ActiveCategory == "Border" ? " active" : "");
            btnTabIcon.CssClass = "shop-category-btn" + (ActiveCategory == "Icon" ? " active" : "");
            btnTabBadge.CssClass = "shop-category-btn" + (ActiveCategory == "Badge" ? " active" : "");

            BindStore();
        }


        protected void rptStore_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "BuyItem")
            {
                if (IsGuest)
                {
                    ShowNotification("🔍 Guest Mode: Please sign in or register an account to redeem store items!");
                    return;
                }

                int itemId;
                if (!int.TryParse(e.CommandArgument.ToString(), out itemId))
                {
                    ShowNotification("Invalid Item.");
                    return;
                }

                var output = ShopService.HandleRedeem(CurrentUser.Id, itemId);
                ShowNotification(output.Message);

                if (output.Success)
                {
                    BindStore();
                    ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
                }
            }
        }

        public string GetCategoryBadgeClass(string category)
        {
            if (string.IsNullOrEmpty(category)) return "category-pill cat-border";
            if (category.Equals("Border", StringComparison.OrdinalIgnoreCase)) return "category-pill cat-border";
            if (category.Equals("Icon", StringComparison.OrdinalIgnoreCase)) return "category-pill cat-icon";
            if (category.Equals("Badge", StringComparison.OrdinalIgnoreCase)) return "category-pill cat-badge";

            return "category-pill cat-border";
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = message;
        }
    }
}