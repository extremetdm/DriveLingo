<%@ Page Title="DriveLingo | Learning Material Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Lessons.aspx.cs" Inherits="DriveLingo.Admin.Lessons" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- PANEL 3: CRUD LEARNING MATERIAL -->
  <asp:Panel ID="pnlMaterials" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">📚 CRUD Learning Materials</h1>
      <p style="color: var(--text-secondary); margin: 0;">Author new JPJ study guides, update handbook categories, and manage PDF manual downloads.</p>
    </div>

    <div class="grid-2-col">
      <!-- Create / Edit Material Form -->
      <div class="glass-card">
        <asp:HiddenField ID="hfEditingMaterialId" runat="server" Value="" />
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">
          <asp:Literal ID="litMaterialFormTitle" runat="server" Text="➕ Add Study Guide Material" />
        </h2>
        <div style="display: flex; flex-direction: column; gap: 1rem;">
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Material Title</label>
            <asp:TextBox ID="txtMatTitle" runat="server" CssClass="form-control" placeholder="Malaysian Speed Limits" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Category</label>
            <asp:DropDownList ID="ddlMatCategory" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
              
            </asp:DropDownList>
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">🚦 Attach Road Sign Image (File Upload or Image Link)</label>
            <asp:FileUpload ID="fileMaterialImage" runat="server" CssClass="form-control" style="width: 100%; padding: 0.5rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white; margin-bottom: 0.5rem;" />
            <asp:TextBox ID="txtMaterialImageUrl" runat="server" CssClass="form-control" placeholder="Or select/paste road sign image path (e.g. uploads/speed_limit_110.svg)..." style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">PDF Manual URL (optional)</label>
            <asp:TextBox ID="txtMatPdf" runat="server" CssClass="form-control" placeholder="https://www.jpj.gov.my/..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Guide Content</label>
            <asp:TextBox ID="txtMatContent" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Full guide text..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div style="display: flex; gap: 0.5rem;">
            <asp:Button ID="btnAddMaterial" runat="server" Text="➕ Create Material" OnClick="btnAddMaterial_Click" CssClass="btn btn-primary" style="flex: 1;" />
            <asp:Button ID="btnCancelMaterialEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelMaterialEdit_Click" Visible="false" CssClass="btn btn-secondary" />
          </div>
        </div>
      </div>

      <!-- Materials List Grid -->
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🗃️ Active Study Guides</h2>
        <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvMaterials_RowCommand" EmptyDataText="No materials found.">
          <Columns>
            <asp:BoundField DataField="Module" HeaderText="Category" ItemStyle-Width="120px" />
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
              <ItemTemplate>
                <asp:Button ID="btnEditMat" runat="server" Text="✏️ Edit" CommandName="EditMaterial" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                <asp:Button ID="btnDeleteMat" runat="server" Text="🗑️" CommandName="DeleteMaterial" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this study material?');" />
              </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>
</asp:Content>
