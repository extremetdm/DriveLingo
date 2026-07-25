<%@ Page Title="DriveLingo Portal Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DriveLingo.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <div class="glass-card" style="margin-bottom: 2rem; text-align: center; padding: 3.5rem 1.5rem;">
    <span style="font-size: 4.5rem; display: block; margin-bottom: 1rem;">🚗</span>
    <h1 style="font-size: 2.8rem; font-family: var(--font-heading); margin-bottom: 1rem;">
      Welcome to DriveLingo ASP.NET Engine
    </h1>
    <p style="color: var(--text-secondary); max-width: 680px; margin: 0 auto 2.5rem auto; line-height: 1.6; font-size: 1.1rem;">
      Official Malaysian JPJ KPP01 Driving Theory Exam Preparation Platform powered by ASP.NET Web Forms, C# Code-Behind event handling, and Entity Framework data persistence.
    </p>

    <!-- Navigation Shortcuts -->
    <div style="display: flex; justify-content: center; gap: 1.25rem; flex-wrap: wrap;">
      <asp:HyperLink ID="lnkLearnerPortal" runat="server" NavigateUrl="~/Dashboard" CssClass="btn btn-primary" style="font-size: 1.1rem; padding: 0.85rem 2rem;">
        🚘 Launch Learner Portal
      </asp:HyperLink>

      <asp:HyperLink ID="lnkEducatorHub" runat="server" NavigateUrl="~/Instructor" CssClass="btn btn-secondary" style="font-size: 1.1rem; padding: 0.85rem 2rem;">
        👨‍✈️ Open Educator Hub
      </asp:HyperLink>

      <asp:HyperLink ID="lnkAdminConsole" runat="server" NavigateUrl="~/Admin" CssClass="btn btn-secondary" style="font-size: 1.1rem; padding: 0.85rem 2rem;">
        👑 Open Admin Console
      </asp:HyperLink>
    </div>
  </div>

  <!-- Role Quick Switch & Features Grid -->
  <div class="grid-3-col">
    <div class="glass-card" style="text-align: center;">
      <span style="font-size: 2.5rem; display: block; margin-bottom: 0.75rem;">🚘</span>
      <h3 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">Candidate Practice</h3>
      <p style="color: var(--text-secondary); font-size: 0.95rem; margin-bottom: 1.5rem;">
        Take timed JPJ KPP01 practice tests, earn XP points, level up, and unlock store items.
      </p>
      <asp:Button ID="btnQuickLearner" runat="server" Text="Log in as Candidate" OnClick="btnQuickLearner_Click" CssClass="btn btn-primary" style="width: 100%;" />
    </div>

    <div class="glass-card" style="text-align: center;">
      <span style="font-size: 2.5rem; display: block; margin-bottom: 0.75rem;">👨‍✈️</span>
      <h3 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">Educator Hub</h3>
      <p style="color: var(--text-secondary); font-size: 0.95rem; margin-bottom: 1.5rem;">
        Create custom quizzes, manage questions via GridView, and answer candidate forum questions.
      </p>
      <asp:Button ID="btnQuickEducator" runat="server" Text="Log in as Educator" OnClick="btnQuickEducator_Click" CssClass="btn btn-secondary" style="width: 100%;" />
    </div>

    <div class="glass-card" style="text-align: center;">
      <span style="font-size: 2.5rem; display: block; margin-bottom: 0.75rem;">👑</span>
      <h3 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">Admin Operations</h3>
      <p style="color: var(--text-secondary); font-size: 0.95rem; margin-bottom: 1.5rem;">
        Manage user roles, audit full question databases, reset application state, and view analytics.
      </p>
      <asp:Button ID="btnQuickAdmin" runat="server" Text="Log in as Admin" OnClick="btnQuickAdmin_Click" CssClass="btn btn-secondary" style="width: 100%;" />
    </div>
  </div>
</asp:Content>
