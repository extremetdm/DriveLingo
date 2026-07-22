<%@ Page Title="DriveLingo | Educator Hub" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Educator.aspx.cs" Inherits="DriveLingo.Educator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- PANEL 1: DASHBOARD OVERVIEW -->
  <asp:Panel ID="pnlDashboard" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">📊 Instructor Dashboard</h1>
      <p style="color: var(--text-secondary); margin: 0;">Overview of active question bank sizes, student attempt volumes, and average pass rates.</p>
    </div>

    <div class="grid-3-col" style="margin-bottom: 2rem;">
      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">❓</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Active Questions</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--primary);"><asp:Literal ID="litTotalQuestionsCount" runat="server" Text="0" /></span>
        </div>
      </div>

      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">📝</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Candidate Attempts</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--warning);"><asp:Literal ID="litTotalAttemptsCount" runat="server" Text="0" /></span>
        </div>
      </div>

      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">🎯</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Average Pass Rate</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--success);"><asp:Literal ID="litAveragePassRate" runat="server" Text="100%" /></span>
        </div>
      </div>
    </div>
  </asp:Panel>

  <!-- PANEL 2: CRUD QUIZ & QUESTION BANK -->
  <asp:Panel ID="pnlQuizzes" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">✍️ CRUD Quiz & Question Bank Manager</h1>
      <p style="color: var(--text-secondary); margin: 0;">Create new practice questions, set option choices, citation notes, and manage existing quiz questions.</p>
    </div>

    <div class="grid-2-col" style="margin-bottom: 2rem;">
      <!-- Quiz Question Creation Form -->
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">➕ Create New Practice Question</h2>

        <div style="display: flex; flex-direction: column; gap: 1.25rem;">
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Target Quiz Category</label>
            <asp:DropDownList ID="ddlQuizTarget" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>

          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Question Text Prompt</label>
            <asp:TextBox ID="txtQuestionText" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="What is the speed limit in school zones?" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>

          <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem;">
            <div>
              <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.85rem;">Option 1 (A)</label>
              <asp:TextBox ID="txtOpt1" runat="server" CssClass="form-control" style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
            </div>
            <div>
              <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.85rem;">Option 2 (B)</label>
              <asp:TextBox ID="txtOpt2" runat="server" CssClass="form-control" style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
            </div>
            <div>
              <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.85rem;">Option 3 (C)</label>
              <asp:TextBox ID="txtOpt3" runat="server" CssClass="form-control" style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
            </div>
            <div>
              <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.85rem;">Option 4 (D)</label>
              <asp:TextBox ID="txtOpt4" runat="server" CssClass="form-control" style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
            </div>
          </div>

          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Correct Answer Key</label>
            <asp:DropDownList ID="ddlCorrectIndex" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
              <asp:ListItem Value="0">Option 1 (A)</asp:ListItem>
              <asp:ListItem Value="1">Option 2 (B)</asp:ListItem>
              <asp:ListItem Value="2">Option 3 (C)</asp:ListItem>
              <asp:ListItem Value="3">Option 4 (D)</asp:ListItem>
            </asp:DropDownList>
          </div>

          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Explanation & Rule Citation</label>
            <asp:TextBox ID="txtExplanation" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Under JPJ school zone guidelines..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>

          <asp:Button ID="btnAddQuestion" runat="server" Text="➕ Save Question to Database" OnClick="btnAddQuestion_Click" CssClass="btn btn-primary" style="padding: 0.85rem; font-weight: 700;" />
        </div>
      </div>

      <!-- Master Question Bank Grid -->
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">🗃️ Active Question Bank Grid</h2>
        <asp:GridView ID="gvQuestions" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowDeleting="gvQuestions_RowDeleting" EmptyDataText="No questions found.">
          <Columns>
            <asp:BoundField DataField="Id" HeaderText="Q ID" ItemStyle-Width="70px" />
            <asp:BoundField DataField="Text" HeaderText="Prompt" />
            <asp:CommandField ShowDeleteButton="true" DeleteText="🗑️ Delete" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" ButtonType="Button" ControlStyle-CssClass="btn btn-secondary btn-sm" />
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>

  <!-- PANEL 3: FORUM MODERATION -->
  <asp:Panel ID="pnlForum" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">💬 Forum Discussion Moderation</h1>
      <p style="color: var(--text-secondary); margin: 0;">Provide educator-verified answers to student questions and answer community inquiries.</p>
    </div>

    <div class="glass-card">
      <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">Answer Student Inquiries</h2>
      <asp:Repeater ID="rptForumModeration" runat="server" OnItemCommand="rptForumModeration_ItemCommand">
        <ItemTemplate>
          <div style="margin-bottom: 1.25rem; padding: 1.25rem; background: rgba(15,23,42,0.4); border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.05);">
            <div style="display: flex; justify-content: space-between; margin-bottom: 0.5rem;">
              <span class="badge" style="background: rgba(99, 102, 241, 0.2); color: var(--primary);"><%# Eval("Category") %></span>
              <span style="font-size: 0.85rem; color: var(--text-secondary);"><%# Eval("DatePosted") %></span>
            </div>
            <h4 style="margin: 0 0 0.5rem 0; font-size: 1.1rem;"><%# Eval("Title") %></h4>
            <p style="color: var(--text-secondary); font-size: 0.9rem; margin-bottom: 1rem;"><%# Eval("Content") %></p>
            
            <div style="display: flex; gap: 0.5rem;">
              <asp:TextBox ID="txtEducatorReply" runat="server" CssClass="form-control" placeholder="Write verified instructor response..." style="flex: 1; padding: 0.6rem; font-size: 0.85rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
              <asp:Button ID="btnSubmitReply" runat="server" Text="Post Answer" CommandName="Reply" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary btn-sm" />
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </asp:Panel>

  <!-- PANEL 4: LEARNER REPORT -->
  <asp:Panel ID="pnlReports" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">📈 Candidate Performance Reports</h1>
      <p style="color: var(--text-secondary); margin: 0;">Comprehensive audit of candidate practice attempts, scores, and exam pass rates.</p>
    </div>

    <div class="glass-card">
      <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">Recent Candidate Test Attempts</h2>
      <asp:GridView ID="gvLearnerReports" runat="server" AutoGenerateColumns="false" CssClass="data-table" EmptyDataText="No learner attempts recorded yet.">
        <Columns>
          <asp:BoundField DataField="UserId" HeaderText="Candidate ID" ItemStyle-Width="120px" />
          <asp:BoundField DataField="QuizTitle" HeaderText="Quiz Name" />
          <asp:BoundField DataField="Score" HeaderText="Score" ItemStyle-Width="80px" />
          <asp:BoundField DataField="Percentage" HeaderText="Percentage" DataFormatString="{0}%" ItemStyle-Width="100px" />
          <asp:TemplateField HeaderText="Result" ItemStyle-Width="100px">
            <ItemTemplate>
              <span class='<%# Convert.ToBoolean(Eval("Passed")) ? "badge badge-success" : "badge badge-danger" %>'>
                <%# Convert.ToBoolean(Eval("Passed")) ? "PASS 🟢" : "FAIL 🔴" %>
              </span>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:BoundField DataField="DateTaken" HeaderText="Date Taken" ItemStyle-Width="140px" />
        </Columns>
      </asp:GridView>
    </div>
  </asp:Panel>
</asp:Content>
