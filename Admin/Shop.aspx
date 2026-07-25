<%@ Page Title="DriveLingo | Shop Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="DriveLingo.Admin.Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Notification Banner -->
    <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
        <asp:Literal ID="litNotificationText" runat="server" />
    </asp:Panel>

    <!-- PANEL 5: CRUD STORE -->
    <asp:Panel ID="pnlStore" runat="server">
        <div class="glass-card" style="margin-bottom: 2rem;">
            <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🛒 CRUD Store Cosmetics & Pricing</h1>
            <p style="color: var(--text-secondary); margin: 0;">Add new avatar frames (Borders), custom icons, and name badges, and configure candidate point prices and custom border colors.</p>
        </div>

        <div class="grid-2-col">
            <!-- Create / Edit Store Item Form -->
            <div class="glass-card">
                <asp:HiddenField ID="hfEditingStoreItemId" runat="server" Value="" />
                <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">
                    <asp:Literal ID="litStoreFormTitle" runat="server" Text="➕ Create Store Item" />
                </h2>

                <div style="display: flex; flex-direction: column; gap: 1rem;">
                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Store Category</label>
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
                            <asp:ListItem Value="Border" Text="✨ Border (Icon Frame)" />
                            <asp:ListItem Value="Icon" Text="🏎️ Custom Avatar Icon" />
                            <asp:ListItem Value="Badge" Text="⚡ Name Badge" />
                        </asp:DropDownList>
                    </div>

                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Item Title / Name</label>
                        <asp:TextBox ID="txtStoreTitle" runat="server" CssClass="form-control" placeholder="Glowing Neon Frame" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                    </div>

                    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 0.75rem;">
                        <div>
                            <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Icon Emoji</label>
                            <asp:TextBox ID="txtStoreIcon" runat="server" Text="✨" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white; text-align: center;" />
                        </div>
                        <div>
                            <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Price (Points)</label>
                            <asp:TextBox ID="txtStorePrice" runat="server" Text="200" TextMode="Number" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                        </div>
                    </div>

                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Border Color Hex (For Border items, e.g. #6366f1, #f59e0b)</label>
                        <asp:TextBox ID="txtColorHex" runat="server" Text="#6366f1" CssClass="form-control" placeholder="#6366f1" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                    </div>

                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Description</label>
                        <asp:TextBox ID="txtStoreDesc" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Glowing profile border outline" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                    </div>

                    <div style="display: flex; gap: 0.5rem; margin-top: 0.5rem;">
                        <asp:Button ID="btnAddStoreItem" runat="server" Text="➕ Create Store Item" OnClick="btnAddStoreItem_Click" CssClass="btn btn-primary" style="flex: 1; padding: 0.85rem; font-weight: 700;" />
                        <asp:Button ID="btnCancelStoreEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelStoreEdit_Click" Visible="false" CssClass="btn btn-secondary" style="padding: 0.85rem;" />
                    </div>
                </div>
            </div>

            <!-- Active Store Items Data Table Grid -->
            <div class="glass-card">
                <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🗃️ Active Store Catalog</h2>
                <asp:GridView ID="gvStore" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvStore_RowCommand" EmptyDataText="No store items found.">
                    <Columns>
                        <asp:BoundField DataField="Icon" HeaderText="Icon" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Category" HeaderText="Category" ItemStyle-Width="90px" />
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="Price" HeaderText="Price (Pts)" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Right">
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
