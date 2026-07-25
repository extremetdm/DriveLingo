<%@ Page Title="DriveLingo | Simulation Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Simulation.aspx.cs" Inherits="DriveLingo.Admin.Simulation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- PANEL 4: CRUD SIMULATION QUESTION BANK -->
  <asp:Panel ID="pnlSimulation" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🏎️ CRUD Simulation Question Bank</h1>
      <p style="color: var(--text-secondary); margin: 0;">Add, edit, or remove questions from the 180+ question pool across Color Blindness, Section A, Section B, and Section C.</p>
    </div>

    <div class="grid-2-col">
      <!-- Question Form Card -->
      <div class="glass-card">
        <asp:HiddenField ID="hfEditingSimQuestionId" runat="server" Value="" />
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">
          <asp:Literal ID="litSimFormTitle" runat="server" Text="➕ Add Simulation Question" />
        </h2>
        <div style="display: flex; flex-direction: column; gap: 1rem;">
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Target Test Section</label>
            <asp:DropDownList ID="ddlSimSection" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
              <asp:ListItem Value="ColorBlindness">👁️ Color Blindness Test (8 Qs)</asp:ListItem>
              <asp:ListItem Value="SectionA">🛑 Section A - Road Signs (21 Qs)</asp:ListItem>
              <asp:ListItem Value="SectionB">🛣️ Section B - Rules of the Road (35 Qs)</asp:ListItem>
              <asp:ListItem Value="SectionC">⚠️ Section C - KEJARA & Safety (14 Qs)</asp:ListItem>
            </asp:DropDownList>
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Question Text</label>
            <asp:TextBox ID="txtSimQuestionText" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Enter question description..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Option 1 (Index 0)</label>
            <asp:TextBox ID="txtSimOpt1" runat="server" CssClass="form-control" placeholder="Choice 1" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Option 2 (Index 1)</label>
            <asp:TextBox ID="txtSimOpt2" runat="server" CssClass="form-control" placeholder="Choice 2" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Option 3 (Index 2)</label>
            <asp:TextBox ID="txtSimOpt3" runat="server" CssClass="form-control" placeholder="Choice 3" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Option 4 (Index 3)</label>
            <asp:TextBox ID="txtSimOpt4" runat="server" CssClass="form-control" placeholder="Choice 4" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Correct Answer Index</label>
            <asp:DropDownList ID="ddlSimCorrect" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
              <asp:ListItem Value="0">Option 1 (Index 0)</asp:ListItem>
              <asp:ListItem Value="1">Option 2 (Index 1)</asp:ListItem>
              <asp:ListItem Value="2">Option 3 (Index 2)</asp:ListItem>
              <asp:ListItem Value="3">Option 4 (Index 3)</asp:ListItem>
            </asp:DropDownList>
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Image Visual URL (optional)</label>
            <asp:TextBox ID="txtSimImageUrl" runat="server" CssClass="form-control" placeholder="uploads/ishihara_12.svg" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Explanation</label>
            <asp:TextBox ID="txtSimExplanation" runat="server" CssClass="form-control" placeholder="Official answer reasoning..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div style="display: flex; gap: 0.5rem;">
            <asp:Button ID="btnAddSimQuestion" runat="server" Text="➕ Create Simulation Question" OnClick="btnAddSimQuestion_Click" CssClass="btn btn-primary" style="flex: 1;" />
            <asp:Button ID="btnCancelSimEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelSimEdit_Click" Visible="false" CssClass="btn btn-secondary" />
          </div>
        </div>
      </div>

      <!-- Simulation Question Bank Grid Card -->
      <div class="glass-card">
        <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; flex-wrap: wrap; gap: 0.5rem;">
          <h2 style="font-family: var(--font-heading); margin: 0;">🗃️ Simulation Question Pool</h2>
          <asp:DropDownList ID="ddlFilterSimSection" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterSimSection_SelectedIndexChanged" CssClass="form-control" style="padding: 0.5rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white; font-size: 0.85rem;">
            <asp:ListItem Value="ALL">All Sections</asp:ListItem>
            <asp:ListItem Value="ColorBlindness">👁️ Color Blindness</asp:ListItem>
            <asp:ListItem Value="SectionA">🛑 Section A - Signs</asp:ListItem>
            <asp:ListItem Value="SectionB">🛣️ Section B - Rules</asp:ListItem>
            <asp:ListItem Value="SectionC">⚠️ Section C - KEJARA</asp:ListItem>
          </asp:DropDownList>
        </div>

        <asp:GridView ID="gvSimQuestions" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvSimQuestions_RowCommand" OnRowDeleting="gvSimQuestions_RowDeleting" EmptyDataText="No simulation questions found.">
          <Columns>
            <asp:BoundField DataField="Section" HeaderText="Section" ItemStyle-Width="120px" />
            <asp:TemplateField HeaderText="Question">
              <ItemTemplate>
                <div style="font-weight: 600;"><%# Eval("Text").ToString().Length > 60 ? Eval("Text").ToString().Substring(0, 60) + "..." : Eval("Text") %></div>
              </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
              <ItemTemplate>
                <asp:Button ID="btnEditSimQ" runat="server" Text="✏️ Edit" CommandName="EditSimQuestion" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                <asp:Button ID="btnDeleteSimQ" runat="server" Text="🗑️" CommandName="DeleteSimQuestion" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this simulation question?');" />
              </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>

</asp:Content>