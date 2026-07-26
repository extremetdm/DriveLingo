<%@ Page Title="DriveLingo | System Administration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DriveLingo.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- PANEL 1: DASHBOARD -->
  <asp:Panel ID="pnlDashboard" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem; display: flex; align-items: center; justify-content: space-between; flex-wrap: wrap; gap: 1rem;">
      <div>
        <h1 style="font-family: var(--font-heading); margin-bottom: 0.25rem;">👑 System Administration & Dashboard</h1>
        <p style="color: var(--text-secondary); margin: 0;">Monitor global databases, manage privileges, audit materials, and run system maintenance.</p>
      </div>
    </div>

    <!-- Metrics Grid -->
    <div class="grid-3-col" style="margin-bottom: 2rem;">
      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">👥</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Registered Accounts</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--primary);"><asp:Literal ID="litTotalUsers" runat="server" Text="0" /></span>
        </div>
      </div>

      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">❓</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Total Questions</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--secondary);"><asp:Literal ID="litTotalQuestions" runat="server" Text="0" /></span>
        </div>
      </div>

      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">📝</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Completed Exams</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--warning);"><asp:Literal ID="litTotalAttempts" runat="server" Text="0" /></span>
        </div>
      </div>
    </div>
  </asp:Panel>
</asp:Content>
