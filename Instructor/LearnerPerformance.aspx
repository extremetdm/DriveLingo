<%@ Page Title="DriveLingo | Learner Performance Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LearnerPerformance.aspx.cs" Inherits="DriveLingo.Instructor.LearnerPerformance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- PANEL 4: LEARNER REPORT -->
  <asp:Panel ID="pnlReports" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">📈 Candidate Performance Reports</h1>
      <p style="color: var(--text-secondary); margin: 0;">Comprehensive audit of candidate practice attempts, scores, and exam pass rates.</p>
    </div>

    <div class="glass-card">
      <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">Recent Candidate Test Attempts</h2>
      <asp:GridView ID="gvLearnerReports" runat="server" AutoGenerateColumns="false" CssClass="data-table" EmptyDataText="No learner attempts recorded yet.">
        <Columns>
          <asp:BoundField DataField="Username" HeaderText="Learner" ItemStyle-Width="120px" />
          <asp:BoundField DataField="QuizTitle" HeaderText="Quiz Name" />
          <asp:BoundField DataField="Score" HeaderText="Score" ItemStyle-Width="80px" />
          <asp:BoundField DataField="Percentage" HeaderText="Percentage" DataFormatString="{0}%" ItemStyle-Width="100px" />
          <asp:TemplateField HeaderText="Result" ItemStyle-Width="100px">
            <ItemTemplate>
              <asp:PlaceHolder runat="server" Visible='<%# Eval("Passed") %>'>
                  <span class="badge badge-success">PASS 🟢</span>
              </asp:PlaceHolder>

              <asp:PlaceHolder runat="server" Visible='<%# !Convert.ToBoolean(Eval("Passed")) %>'>
                <span class="badge badge-danger">FAIL 🔴</span>
              </asp:PlaceHolder>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:BoundField DataField="CompletedAt" HeaderText="Date Taken" ItemStyle-Width="140px" />
        </Columns>
      </asp:GridView>
    </div>
  </asp:Panel>
</asp:Content>

