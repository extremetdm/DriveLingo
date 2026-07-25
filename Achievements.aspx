<%@ Page Title="DriveLingo | Achievements" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Achievements.aspx.cs" Inherits="DriveLingo.Achievements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <!-- Notification Banner -->
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- TAB 7: ACHIEVEMENTS -->
  <asp:Panel ID="pnlAchievements" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h2 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🏆 Badges & Achievements Tracker</h2>
      <p style="color: var(--text-secondary); margin: 0;">Unlock milestones by taking practice exams, scoring high marks, and studying JPJ materials.</p>
    </div>

    <div class="grid-3-col">
      <asp:Repeater ID="rptAchievements" runat="server" OnItemDataBound="rptAchievements_ItemDataBound">
        <ItemTemplate>
          <div class="glass-card" style="text-align: center;">
            <span style="font-size: 3.5rem; display: block; margin-bottom: 0.5rem;"><%# Eval("Icon") %></span>
            <h3 style="font-family: var(--font-heading); margin-bottom: 0.25rem;"><%# Eval("Title") %></h3>
            <p style="color: var(--text-secondary); font-size: 0.9rem; margin-bottom: 1rem;"><%# Eval("Description") %></p>
            <asp:Label ID="lblAchievementStatus" runat="server" CssClass="badge" />
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </asp:Panel>
</asp:Content>
