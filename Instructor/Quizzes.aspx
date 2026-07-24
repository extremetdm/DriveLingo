<%@ Page Title="DriveLingo | Quiz Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Quizzes.aspx.cs" Inherits="DriveLingo.Instructor.Quizzes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- PANEL 2: CRUD QUIZ & QUESTION BANK -->
  <asp:Panel ID="pnlQuizzes" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">✍️ CRUD Quiz & Question Bank Manager</h1>
      <p style="color: var(--text-secondary); margin: 0;">Create new practice questions, set option choices, citation notes, and manage existing quiz questions.</p>
    </div>

    <div class="grid-2-col" style="margin-bottom: 2rem;">
      <!-- Quiz Question Creation / Editing Form -->
      <div class="glass-card">
        <asp:HiddenField ID="hfEditingQuestionId" runat="server" Value="" />
        <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">
          <asp:Literal ID="litFormTitle" runat="server" Text="➕ Create New Practice Question" />
        </h2>

        <div style="display: flex; flex-direction: column; gap: 1.25rem;">
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Target Quiz Category</label>
            <asp:DropDownList ID="ddlQuizTarget" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>

          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Question Text Prompt</label>
            <asp:TextBox ID="txtQuestionText" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="What is the speed limit in school zones?" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>

          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">🚦 Attach Road Sign Image (File Upload or Image Link)</label>
            <asp:FileUpload ID="fileQuestionImage" runat="server" CssClass="form-control" style="width: 100%; padding: 0.5rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white; margin-bottom: 0.5rem;" />
            <asp:TextBox ID="txtQuestionImageUrl" runat="server" CssClass="form-control" placeholder="Or select/paste road sign image path (e.g. uploads/no_entry.svg)..." style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>

          <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem;">
              <asp:Repeater ID="rptChoices" runat="server">
                <ItemTemplate>
                  <div>
                    <asp:HiddenField ID="hfChoiceId" runat="server" Value='<%# Eval("Id") %>' />

                    <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.85rem;">
                      Option <%# Container.ItemIndex + 1 %> (<%# (char)('A' + Container.ItemIndex) %>)
                    </label>

                    <asp:TextBox ID="txtChoiceText" runat="server" 
                      Text='<%# Eval("Text") %>' 
                      CssClass="form-control" 
                      style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                  </div>
                </ItemTemplate>
              </asp:Repeater>
            </div>

          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Correct Answer Key</label>
            <asp:DropDownList ID="ddlCorrectIndex" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
               
            </asp:DropDownList>
          </div>

          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Explanation & Rule Citation</label>
            <asp:TextBox ID="txtExplanation" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Under JPJ school zone guidelines..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>

          <div style="display: flex; gap: 0.5rem;">
            <asp:Button ID="btnAddQuestion" runat="server" Text="➕ Save Question to Database" OnClick="btnAddQuestion_Click" CssClass="btn btn-primary" style="flex: 1; padding: 0.85rem; font-weight: 700;" />
            <asp:Button ID="btnCancelQuestionEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelQuestionEdit_Click" Visible="false" CssClass="btn btn-secondary" style="padding: 0.85rem;" />
          </div>
        </div>
      </div>

      <!-- Master Question Bank Grid -->
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">🗃️ Active Question Bank Grid</h2>
        <asp:GridView ID="gvQuestions" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvQuestions_RowCommand" EmptyDataText="No questions found.">
          <Columns>
            <asp:BoundField DataField="Id" HeaderText="Q ID" ItemStyle-Width="60px" />
            <asp:BoundField DataField="Text" HeaderText="Prompt" />
            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
              <ItemTemplate>
                <asp:Button ID="btnEditQ" runat="server" Text="✏️ Edit" CommandName="EditQuestion" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                <asp:Button ID="btnDeleteQ" runat="server" Text="🗑️" CommandName="DeleteQuestion" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this question?');" />
              </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>
</asp:Content>

