<%@ Page Title="DriveLingo | Shop" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="DriveLingo.Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <!-- Notification Banner -->
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- TAB 4: STORE -->
  <asp:Panel ID="pnlStore" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h2 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🛒 Candidate Points Marketplace</h2>
      <p style="color: var(--text-secondary); margin: 0;">Redeem your hard-earned quiz points for profile borders, badges, and exclusive visual themes.</p>
    </div>

    <div class="grid-3-col">
      <asp:Repeater ID="rptStore" runat="server" OnItemCommand="rptStore_ItemCommand" OnItemDataBound="rptStore_ItemDataBound">
        <ItemTemplate>
          <div class="glass-card" style="text-align: center; display: flex; flex-direction: column; justify-content: space-between;">
            <div>
              <span style="font-size: 3.5rem; display: block; margin-bottom: 0.5rem;"><%# Eval("Icon") %></span>
              <h3 style="font-family: var(--font-heading); margin-bottom: 0.25rem;"><%# Eval("Name") %></h3>
              <span class="badge" style="background: rgba(245, 158, 11, 0.2); color: var(--warning); margin-bottom: 0.75rem; display: inline-block;">
                🪙 <%# Eval("Cost") %> Points
              </span>
              <p style="color: var(--text-secondary); font-size: 0.9rem;"><%# Eval("Description") %></p>
            </div>
            
            <asp:Panel ID="pnlStoreAction" runat="server" style="margin-top: 1.5rem;">
              <!-- If unowned -->
              <asp:Button ID="btnBuyItem" runat="server" Text="Redeem Item" CommandName="BuyItem" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary" style="width: 100%;" />
              <!-- If owned -->
              <asp:Label ID="lblOwnedItem" runat="server" Text="Owned ✔" CssClass="badge badge-success" style="font-size: 1rem; padding: 0.6rem 1.2rem; display: block; width: 100%;" Visible="false" />
            </asp:Panel>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </asp:Panel>
</asp:Content>
