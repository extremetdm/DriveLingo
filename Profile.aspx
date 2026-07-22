<%@ Page Title="DriveLingo | Profile Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="DriveLingo.UserProfilePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <div style="max-width: 720px; margin: 0 auto;">
    <!-- Profile Card Header -->
    <div class="glass-card" style="margin-bottom: 2rem; display: flex; align-items: center; gap: 2rem; flex-wrap: wrap;">
      <div style="font-size: 5rem; width: 100px; height: 100px; background: rgba(99, 102, 241, 0.2); border-radius: 50%; display: flex; align-items: center; justify-content: center; border: 2px solid var(--primary);">
        <asp:Literal ID="litAvatar" runat="server" Text="🚗" />
      </div>

      <div style="flex: 1;">
        <span class="badge" style="background: rgba(99, 102, 241, 0.2); color: var(--primary); margin-bottom: 0.5rem; display: inline-block;">
          <asp:Literal ID="litRoleBadge" runat="server" Text="LEARNER" />
        </span>
        <h1 style="font-family: var(--font-heading); margin-bottom: 0.25rem;">
          <asp:Literal ID="litUserName" runat="server" Text="Alex Hero" />
        </h1>
        <p style="color: var(--text-secondary); margin: 0;">
          <asp:Literal ID="litUserEmail" runat="server" Text="learner@drivelingo.com" /> | Joined <asp:Literal ID="litJoinedDate" runat="server" Text="2026-07-01" />
        </p>
      </div>
    </div>

    <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
      <asp:Literal ID="litNotificationText" runat="server" />
    </asp:Panel>

    <!-- Profile Edit Form -->
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">⚙️ Profile & Avatar Settings</h2>

      <div style="display: flex; flex-direction: column; gap: 1.25rem;">
        <div>
          <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Display Name</label>
          <asp:TextBox ID="txtName" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
        </div>

        <div>
          <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Avatar Icon Emoji</label>
          <asp:DropDownList ID="ddlAvatar" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
            <asp:ListItem Value="🚗">🚗 Race Car</asp:ListItem>
            <asp:ListItem Value="🏎️">🏎️ Sports Racer</asp:ListItem>
            <asp:ListItem Value="👨‍✈️">👨‍✈️ JPJ Inspector</asp:ListItem>
            <asp:ListItem Value="👑">👑 Gold Crown</asp:ListItem>
            <asp:ListItem Value="⚡">⚡ Speedmaster</asp:ListItem>
          </asp:DropDownList>
        </div>

        <div>
          <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">New Password (leave blank to keep current)</label>
          <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
        </div>

        <asp:Button ID="btnSaveProfile" runat="server" Text="Save Profile Changes" OnClick="btnSaveProfile_Click" CssClass="btn btn-primary" style="padding: 0.85rem; font-weight: 700;" />
      </div>
    </div>

    <!-- Inventory & Unlocked Cosmetics Showcase -->
    <div class="glass-card">
      <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🎒 Owned Items & Cosmetics Inventory</h2>
      <asp:Repeater ID="rptInventory" runat="server" OnItemCommand="rptInventory_ItemCommand" OnItemDataBound="rptInventory_ItemDataBound">
        <HeaderTemplate>
          <div style="display: flex; gap: 1rem; flex-wrap: wrap;">
        </HeaderTemplate>
        <ItemTemplate>
          <div style="display: flex; align-items: center; justify-content: space-between; padding: 0.75rem 1rem; background: rgba(15, 23, 42, 0.4); border-radius: var(--radius-sm); width: 100%; border: 1px solid rgba(255,255,255,0.05);">
            <span style="font-weight: 600; font-size: 0.95rem;">
              ✨ <%# Container.DataItem %>
            </span>
            <asp:Button ID="btnEquipItem" runat="server" 
              CommandName="EquipItem" CommandArgument='<%# Container.DataItem %>' />
          </div>
        </ItemTemplate>
        <FooterTemplate>
          </div>
        </FooterTemplate>
      </asp:Repeater>
      <asp:Label ID="lblNoInventory" runat="server" Text="No store cosmetics unlocked yet. Visit the Candidate Store to redeem items!" Visible="false" style="color: var(--text-secondary);" />
    </div>
  </div>
</asp:Content>
