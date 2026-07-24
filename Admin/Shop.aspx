<%@ Page Title="DriveLingo | Shop Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="DriveLingo.Admin.Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- PANEL 5: CRUD STORE -->
  <asp:Panel ID="pnlStore" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🛒 CRUD Store Cosmetics & Pricing</h1>
      <p style="color: var(--text-secondary); margin: 0;">Add new avatar frames, badges, or visual themes and manage point prices.</p>
    </div>

    <div class="grid-2-col">
      <div class="glass-card">
        <asp:HiddenField ID="hfEditingStoreItemId" runat="server" Value="" />
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">
          <asp:Literal ID="litStoreFormTitle" runat="server" Text="➕ Create Store Item" />
        </h2>
        <div style="display: flex; flex-direction: column; gap: 1rem;">
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Item Title</label>
            <asp:TextBox ID="txtStoreTitle" runat="server" CssClass="form-control" placeholder="Border: Cyberpunk" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Icon Emoji</label>
            <asp:TextBox ID="txtStoreIcon" runat="server" Text="✨" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Price (Points)</label>
            <asp:TextBox ID="txtStorePrice" runat="server" Text="200" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Description</label>
            <asp:TextBox ID="txtStoreDesc" runat="server" CssClass="form-control" placeholder="Glowing profile border" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div style="display: flex; gap: 0.5rem;">
            <asp:Button ID="btnAddStoreItem" runat="server" Text="➕ Create Store Item" OnClick="btnAddStoreItem_Click" CssClass="btn btn-primary" style="flex: 1;" />
            <asp:Button ID="btnCancelStoreEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelStoreEdit_Click" Visible="false" CssClass="btn btn-secondary" />
          </div>
        </div>
      </div>

      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🗃️ Active Store Catalog</h2>
        <asp:GridView ID="gvStore" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvStore_RowCommand" EmptyDataText="No store items found.">
          <Columns>
            <asp:BoundField DataField="Icon" HeaderText="Icon" ItemStyle-Width="50px" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Cost" HeaderText="Price (Pts)" ItemStyle-Width="100px" />
            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
              <ItemTemplate>
                <asp:Button ID="btnEditStore" runat="server" Text="✏️ Edit" CommandName="EditStoreItem" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                <asp:Button ID="btnDeleteStore" runat="server" Text="🗑️" CommandName="DeleteStoreItem" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this store item?');" />
              </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>
</asp:Content>
