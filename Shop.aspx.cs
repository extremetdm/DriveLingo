using DriveLingo.Database;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();

            if (!IsPostBack)
            {
                BindStore();
            }
        }

        // --- Store Handlers ---
        struct ShopAvailableItem
        {
            public int Id { get; set; }
            public string Icon { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Cost { get; set; }
            public bool Owned { get; set; }

        }
        private void BindStore()
        {
            using (var db = new AppDbContext())
            {
                rptStore.DataSource = db.ShopItems
                    .Include(i => i.Redemptions)
                    .ToList()
                    .Select(i => new ShopAvailableItem
                    {
                        Id = i.Id,
                        Icon = i.Icon,
                        Name = i.Name,
                        Description = i.Description,
                        Cost = i.Cost,
                        Owned = i.Redemptions.Any(r => r.UserId == CurrentUser.Id)
                    })
                    .ToList();
                rptStore.DataBind();
            }
        }
        protected void rptStore_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "BuyItem")
            {
                int itemId = int.Parse(e.CommandArgument.ToString());

                var output = ShopService.HandleRedeem(CurrentUser.Id, itemId);
                ShowNotification(output.Message);

                if (output.Success)
                {
                    BindStore();
                    ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
                }
            }
        }

        protected void rptStore_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item
                && e.Item.ItemType != ListItemType.AlternatingItem
            ) return;

            var item = (ShopAvailableItem)e.Item.DataItem;
            Button btnBuyItem = (Button)e.Item.FindControl("btnBuyItem");
            Label lblOwnedItem = (Label)e.Item.FindControl("lblOwnedItem");

            btnBuyItem.Visible = !item.Owned;
            lblOwnedItem.Visible = item.Owned;
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
            // todo add error msg notification
        }
    }
}