<%@ Page Title="DriveLingo | Lessons" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Lessons.aspx.cs" Inherits="DriveLingo.Lessons" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <!-- Notification Banner -->
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- TAB 3: STUDY MATERIALS -->
  <asp:Panel ID="pnlMaterials" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h2 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">📚 Malaysian JPJ Theory Study Guides</h2>
      <p style="color: var(--text-secondary); margin: 0;">Official KPP01 handbook guides, traffic signs diagrams, and RPK/RSM checklists.</p>
    </div>

    <!-- Material Cards Grid View -->
    <asp:Panel ID="pnlMaterialList" runat="server">
      <div class="grid-3-col">
        <asp:Repeater ID="rptMaterials" runat="server" OnItemCommand="rptMaterials_ItemCommand" OnItemDataBound="rptMaterials_ItemDataBound">
          <ItemTemplate>
            <div class="glass-card" style="display: flex; flex-direction: column; justify-content: space-between;">
              <div>
                <div style="display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 0.75rem;">
                  <span class="badge" style="background: rgba(16, 185, 129, 0.2); color: var(--success); display: inline-block;">
                    <%# Eval("Module") %>
                  </span>
                  <asp:PlaceHolder ID="phReadBadge" runat="server" Visible="false">
                    <span class="badge" style="background: rgba(16, 185, 129, 0.3); color: var(--success); font-weight: 700;">Read ✔</span>
                  </asp:PlaceHolder>
                </div>
                <h3 style="font-family: var(--font-heading); margin-bottom: 0.5rem;"><%# Eval("Title") %></h3>

                <%# Eval("Image") != null && !string.IsNullOrEmpty(Eval("Image").ToString()) ? "<div style='text-align: center; margin: 0.75rem 0; padding: 0.75rem; background: rgba(0,0,0,0.3); border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.05);'><img src='" + Eval("Image") + "' alt='Road Sign Visual' style='max-height: 120px; max-width: 100%; border-radius: 6px;' /></div>" : "" %>

                <p style="color: var(--text-secondary); font-size: 0.85rem; margin-bottom: 1rem;">
                  ⏱️ <%# Eval("EstimatedTime") %> min
                </p>
                <p style="color: var(--text-secondary); font-size: 0.95rem; line-height: 1.5;">
                  <%# Eval("Content").ToString().Length > 120 ? Eval("Content").ToString().Substring(0, 120) + "..." : Eval("Content") %>
                </p>
              </div>
              <div style="margin-top: 1.5rem; display: flex; gap: 0.5rem;">
                <asp:Button ID="btnReadMaterial" runat="server" Text="📖 Read Guide" CommandName="ReadMaterial" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary btn-sm" style="flex: 1;" />
                <asp:HyperLink ID="lnkPdf" runat="server" 
                    NavigateUrl='<%# Eval("Pdf") %>' 
                    Target="_blank" 
                    CssClass="btn btn-secondary btn-sm" 
                    Visible='<%# !string.IsNullOrEmpty(Eval("Pdf") as string) %>'>
                    📄 PDF
                </asp:HyperLink>
              </div>
            </div>
          </ItemTemplate>
        </asp:Repeater>
      </div>
    </asp:Panel>

    <!-- Full Material Reader / Expanded Detail View -->
    <asp:Panel ID="pnlMaterialDetail" runat="server" Visible="false" CssClass="glass-card" style="margin-top: 1rem;">
      <div style="display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 1.5rem; border-bottom: 1px solid rgba(255,255,255,0.1); padding-bottom: 1rem; flex-wrap: wrap; gap: 1rem;">
        <div>
          <span class="badge" style="background: rgba(16, 185, 129, 0.2); color: var(--success); margin-bottom: 0.5rem; display: inline-block;">
            <asp:Literal ID="litMatCategory" runat="server" />
          </span>
          <h2 style="font-family: var(--font-heading); margin: 0.25rem 0;">
            <asp:Literal ID="litMatTitle" runat="server" />
          </h2>
          <span style="color: var(--text-secondary); font-size: 0.85rem;">⏱️ <asp:Literal ID="litMatReadTime" runat="server" /></span>
        </div>
        <asp:Button ID="btnCloseMaterialDetail" runat="server" Text="❌ Back to Guides" OnClick="btnCloseMaterialDetail_Click" CssClass="btn btn-secondary" />
      </div>

      <asp:PlaceHolder ID="phMatImage" runat="server" Visible="false">
        <div style="text-align: center; margin: 1.5rem 0; padding: 1.5rem; background: rgba(0,0,0,0.3); border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.1);">
          <asp:Image ID="imgMatDetail" runat="server" style="max-height: 250px; max-width: 100%; border-radius: 8px; box-shadow: 0 4px 15px rgba(0,0,0,0.5);" />
        </div>
      </asp:PlaceHolder>

      <div style="background: rgba(15, 23, 42, 0.5); padding: 1.5rem; border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.05); font-size: 1.05rem; line-height: 1.8; color: var(--text-primary); margin-bottom: 1.5rem; white-space: pre-line;">
        <asp:Literal ID="litMatContent" runat="server" />
      </div>

      <div style="display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 1rem;">
        <asp:Panel ID="pnlMatXpNotice" runat="server" CssClass="badge" style="background: rgba(245, 158, 11, 0.2); color: var(--warning); font-size: 0.95rem; padding: 0.6rem 1rem;">
          ⭐ <asp:Literal ID="litMatXpStatus" runat="server" Text="+15 XP Earned for completing this guide!" />
        </asp:Panel>
        <asp:HyperLink ID="hlMatPdf" runat="server" Target="_blank" CssClass="btn btn-primary" Visible="false">📄 Download Official PDF Manual</asp:HyperLink>
      </div>
    </asp:Panel>
  </asp:Panel>
</asp:Content>
