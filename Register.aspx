<%@ Page Title="DriveLingo | Candidate Registration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="DriveLingo.Register" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  <style>
    .auth-wrapper {
      min-height: 80vh;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 2rem 1rem;
    }
    
    .auth-card {
      width: 100%;
      max-width: 460px;
      background: rgba(30, 41, 59, 0.7);
      backdrop-filter: blur(16px);
      -webkit-backdrop-filter: blur(16px);
      border: 1px solid rgba(255, 255, 255, 0.1);
      border-radius: var(--radius-lg);
      padding: 2.5rem 2rem;
      box-shadow: 0 20px 50px rgba(0, 0, 0, 0.4), 0 0 30px rgba(99, 102, 241, 0.15);
    }

    .auth-brand {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 0.75rem;
      font-size: 1.8rem;
      font-family: var(--font-heading);
      font-weight: 800;
      color: white;
      margin-bottom: 0.5rem;
    }

    .auth-sub {
      color: var(--text-secondary);
      font-size: 0.95rem;
      text-align: center;
      margin-bottom: 2rem;
    }

    .form-group {
      margin-bottom: 1.25rem;
    }

    .form-group label {
      display: block;
      font-weight: 600;
      margin-bottom: 0.5rem;
      font-size: 0.85rem;
      color: var(--text-primary);
    }

    .form-input {
      width: 100%;
      padding: 0.85rem 1rem;
      border-radius: var(--radius-sm);
      border: 1px solid rgba(255, 255, 255, 0.12);
      background: rgba(15, 23, 42, 0.7);
      color: white;
      font-size: 0.95rem;
      outline: none;
      transition: all 0.2s ease;
    }

    .form-input:focus {
      border-color: var(--primary);
      box-shadow: 0 0 12px var(--primary-glow);
    }

    .btn-auth-submit {
      width: 100%;
      padding: 0.9rem;
      font-size: 1rem;
      font-weight: 700;
      border-radius: var(--radius-sm);
      margin-top: 0.5rem;
      cursor: pointer;
    }

    .divider {
      border: none;
      border-top: 1px solid rgba(255, 255, 255, 0.1);
      margin: 1.75rem 0 1.25rem 0;
    }
  </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <div class="auth-wrapper">
    <div class="auth-card">
      <div style="text-align: center; margin-bottom: 1.5rem;">
        <div class="auth-brand">
          <span style="font-size: 2.2rem;">🚘</span>
          <span>DriveLingo</span>
        </div>
        <p class="auth-sub">Create New Candidate Account</p>
      </div>

      <asp:Panel ID="pnlError" runat="server" Visible="false" style="border: 1px solid var(--danger); background: rgba(239, 68, 68, 0.15); border-radius: var(--radius-sm); margin-bottom: 1.5rem; padding: 0.85rem 1rem;">
        <span style="color: var(--danger); font-weight: 600; font-size: 0.9rem;">⚠️ <asp:Literal ID="litErrorMsg" runat="server" /></span>
      </asp:Panel>

      <!-- Registration Form -->
      <div>
        <div class="form-group">
          <label>Full Name</label>
          <asp:TextBox ID="txtName" runat="server" CssClass="form-input" placeholder="Ahmad Zaki" />
        </div>

        <div class="form-group">
          <label>Email Address</label>
          <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" placeholder="ahmad@example.com" />
        </div>

        <div class="form-group">
          <label>Password</label>
          <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-input" placeholder="••••••••" />
        </div>



        <asp:Button ID="btnRegisterSubmit" runat="server" Text="Create Account 🚀" OnClick="btnRegisterSubmit_Click" CssClass="btn btn-primary btn-auth-submit" />
      </div>

      <div class="divider"></div>

      <div style="text-align: center;">
        <span style="color: var(--text-secondary); font-size: 0.88rem;">Already registered? </span>
        <a href="Login.aspx" style="color: var(--primary); font-weight: 700; text-decoration: none; font-size: 0.88rem;">Sign In Here</a>
      </div>
    </div>
  </div>
</asp:Content>