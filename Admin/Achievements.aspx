<%@ Page Title="DriveLingo | Achievement Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Achievements.aspx.cs" Inherits="DriveLingo.Admin.Achievements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- PANEL 6: CRUD ACHIEVEMENTS -->
  <asp:Panel ID="pnlAchievements" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🏆 CRUD System Achievements</h1>
      <p style="color: var(--text-secondary); margin: 0;">Define achievement milestones and reward XP bonuses for candidate milestones.</p>
    </div>

    <div class="grid-2-col">
      <div class="glass-card">
        <asp:HiddenField ID="hfEditingAchId" runat="server" Value="" />
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">
          <asp:Literal ID="litAchFormTitle" runat="server" Text="➕ Create Achievement" />
        </h2>
        <div style="display: flex; flex-direction: column; gap: 1rem;">
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Achievement Title</label>
            <asp:TextBox ID="txtAchTitle" runat="server" CssClass="form-control" placeholder="Theory Expert" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Icon Emoji</label>
            <asp:TextBox ID="txtAchIcon" runat="server" Text="🏆" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">XP Bonus</label>
            <asp:TextBox ID="txtAchXp" runat="server" Text="100" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Description</label>
            <asp:TextBox ID="txtAchDesc" runat="server" CssClass="form-control" placeholder="Complete 5 JPJ practice tests" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div style="display: flex; gap: 0.5rem;">
            <asp:Button ID="btnAddAch" runat="server" Text="➕ Create Achievement" OnClick="btnAddAch_Click" CssClass="btn btn-primary" style="flex: 1;" />
            <asp:Button ID="btnCancelAchEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelAchEdit_Click" Visible="false" CssClass="btn btn-secondary" />
          </div>
        </div>
      </div>

      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🗃️ Active Achievements</h2>
        <asp:GridView ID="gvAchievements" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvAchievements_RowCommand" EmptyDataText="No achievements found.">
          <Columns>
            <asp:BoundField DataField="Icon" HeaderText="Icon" ItemStyle-Width="50px" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Xp" HeaderText="XP Awarded" ItemStyle-Width="90px" />
            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
              <ItemTemplate>
                <asp:Button ID="btnEditAch" runat="server" Text="✏️ Edit" CommandName="EditAchievement" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                <asp:Button ID="btnDeleteAch" runat="server" Text="🗑️" CommandName="DeleteAchievement" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this achievement?');" />
              </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>
</asp:Content>
