<%@ Page Title="DriveLingo | Learner Portal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="DriveLingo.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <!-- Notification Banner -->
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- TAB 1: DASHBOARD -->
  <asp:Panel ID="pnlDashboard" runat="server">
    <div class="grid-3-col" style="margin-bottom: 2rem;">
      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">🎯</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Exam Readiness</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--success);"><asp:Literal ID="litPassRate" runat="server" Text="100%" /></span>
        </div>
      </div>

      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">⭐</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Current Level</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--primary);">Level <asp:Literal ID="litLevel" runat="server" Text="2" /></span>
        </div>
      </div>

      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">🪙</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Reward Points</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--warning);"><asp:Literal ID="litPoints" runat="server" Text="350" /> Pts</span>
        </div>
      </div>
    </div>

    <!-- Recent Exam Attempts -->
    <div class="glass-card">
      <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">📝 Recent Quiz Attempts</h2>
      <asp:GridView ID="gvAttempts" runat="server" AutoGenerateColumns="false" CssClass="data-table" EmptyDataText="No exam attempts recorded yet. Launch the simulator to take your first test!">
        <Columns>
          <asp:BoundField DataField="QuizTitle" HeaderText="Practice Test" />
          <asp:BoundField DataField="Score" HeaderText="Score" ItemStyle-Width="80px" />
          <asp:BoundField DataField="Percentage" HeaderText="Percentage" DataFormatString="{0}%" ItemStyle-Width="100px" />
          <asp:TemplateField HeaderText="Result" ItemStyle-Width="100px">
            <ItemTemplate>
              <span class='<%# Convert.ToBoolean(Eval("Passed")) ? "badge badge-success" : "badge badge-danger" %>'>
                <%# Convert.ToBoolean(Eval("Passed")) ? "PASS 🟢" : "FAIL 🔴" %>
              </span>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:BoundField DataField="CompletedAt" HeaderText="Date" ItemStyle-Width="140px" />
        </Columns>
      </asp:GridView>
    </div>
  </asp:Panel>
</asp:Content>
