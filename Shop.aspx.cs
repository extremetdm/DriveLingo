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
    public partial class Shop : AuthPage
    {
        public class ShopItemViewModel
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public string Icon { get; set; }
            public string Category { get; set; }
            public bool Owned { get; set; }
        }

        private string ActiveCategory
        {
            get => ViewState["ActiveCategory"] as string ?? "ALL";
            set => ViewState["ActiveCategory"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();

            if (!IsPostBack)
            {
                BindStore();
            }
        }

        private void BindStore()
        {
            var state = AppStateRepository.GetCurrent();
            var user = Session["CurrentUser"] as User ?? state.Users.FirstOrDefault(u => u.Role == "learner") ?? new User();

            var query = state.StoreItems.AsEnumerable();

            if (ActiveCategory != "ALL")
            {
                query = query.Where(i => i.Category.Equals(ActiveCategory, StringComparison.OrdinalIgnoreCase));
            }

            var list = query.Select(i => new ShopItemViewModel
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Price = i.Price,
                Icon = i.Icon,
                Category = i.Category,
                Owned = user.Inventory != null && user.Inventory.Contains(i.Id)
            }).ToList();

            rptStore.DataSource = list;
            rptStore.DataBind();
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

                var state = AppStateRepository.GetCurrent();
                var user = Session["CurrentUser"] as User;
                if (user == null)
                {
                    string email = CurrentUser != null ? CurrentUser.Email : "";
                    user = state.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                        ?? state.Users.FirstOrDefault(u => u.Role == "learner")
                        ?? new User { Id = "usr_learner", Name = "Candidate", Role = "learner", Points = 500 };
                    Session["CurrentUser"] = user;
                }

                string itemId = e.CommandArgument.ToString();
                var item = state.StoreItems.FirstOrDefault(i => i.Id == itemId);

                if (item == null) return;

                if (user.Points < item.Price)
                {
                    ShowNotification("❌ Insufficient Points! You need " + item.Price + " Pts, but only have " + user.Points + " Pts.");
                    return;
                }

                user.Points -= item.Price;
                if (user.Inventory == null) user.Inventory = new List<string>();
                if (!user.Inventory.Contains(item.Id))
                {
                    user.Inventory.Add(item.Id);
                }

                Session["CurrentUser"] = user;
                ShowNotification("🎉 Successfully redeemed " + item.Title + "! Visit your Profile to equip it.");

                BindStore();
                if (Master is SiteMaster masterPage)
                {
                    masterPage.UpdateUserHeaderAndNavigation();
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