<%@ Page Title="DriveLingo | Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DriveLingo.Login" %>

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
      max-width: 440px;
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
      margin: 2rem 0 1.5rem 0;
    }

    .preset-title {
      font-size: 0.75rem;
      color: var(--text-secondary);
      display: block;
      margin-bottom: 0.75rem;
      text-transform: uppercase;
      letter-spacing: 1px;
      font-weight: 700;
      text-align: center;
    }

    .preset-grid {
      display: grid;
      grid-template-columns: repeat(3, 1fr);
      gap: 0.5rem;
    }
  </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <div class="auth-wrapper">
    <div class="auth-card">
      <div style="text-align: center; margin-bottom: 1.5rem;">
        <div class="auth-brand">
          <span style="font-size: 2.2rem;">🚗</span>
          <span>DriveLingo</span>
        </div>
        <p class="auth-sub">Malaysian JPJ KPP01 Theory Exam Simulator</p>
      </div>

      <asp:Panel ID="pnlError" runat="server" Visible="false" style="border: 1px solid var(--danger); background: rgba(239, 68, 68, 0.15); border-radius: var(--radius-sm); margin-bottom: 1.5rem; padding: 0.85rem 1rem;">
        <span style="color: var(--danger); font-weight: 600; font-size: 0.9rem;">⚠️ <asp:Literal ID="litErrorMsg" runat="server" /></span>
      </asp:Panel>

      <!-- Login Form Wrapper (Toggleable) -->
      <div id="loginFields">
        <div class="form-group">
          <label>Email Address</label>
          <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" placeholder="learner@drivelingo.com" />
        </div>

        <div class="form-group">
          <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 0.5rem;">
            <label style="margin-bottom: 0;">Password</label>
            <a href="javascript:void(0)" onclick="toggleForgotPassword(true)" style="color: var(--primary); font-size: 0.82rem; text-decoration: none; font-weight: 600; transition: color 0.2s;" onmouseover="this.style.color='white'" onmouseout="this.style.color='var(--primary)'">Forgot Password?</a>
          </div>
          <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-input" placeholder="••••••••" />
        </div>

        <asp:Button ID="btnLoginSubmit" runat="server" Text="Sign In to Portal 🚀" OnClick="btnLoginSubmit_Click" CssClass="btn btn-primary btn-auth-submit" />

        <div style="text-align: center; margin-top: 1.25rem;">
          <span style="color: var(--text-secondary); font-size: 0.88rem;">Don't have a candidate account? </span>
          <a href="Register.aspx" style="color: var(--primary); font-weight: 700; text-decoration: none; font-size: 0.88rem;">Create One Here</a>
        </div>

        <div style="margin-top: 1rem;">
          <asp:Button ID="btnContinueGuest" runat="server" Text="🔍 Continue as Guest (Limited Access)" OnClick="btnContinueGuest_Click" CssClass="btn btn-secondary" style="width: 100%; font-weight: 700; background: rgba(15, 23, 42, 0.6); border-color: rgba(255, 255, 255, 0.15);" />
        </div>

        <div class="divider"></div>

        <!-- Quick Demo Login Presets -->
        <div>
          <span class="preset-title">⚡ Developer Quick Login</span>
          <div class="preset-grid">
            <asp:Button ID="btnDemoLearner" runat="server" Text="🚘 Candidate" OnClick="btnDemoLearner_Click" CssClass="btn btn-secondary btn-sm" style="font-size: 0.8rem; padding: 0.5rem;" />
            <asp:Button ID="btnDemoEducator" runat="server" Text="👨‍✈️ Educator" OnClick="btnDemoEducator_Click" CssClass="btn btn-secondary btn-sm" style="font-size: 0.8rem; padding: 0.5rem;" />
            <asp:Button ID="btnDemoAdmin" runat="server" Text="👑 Admin" OnClick="btnDemoAdmin_Click" CssClass="btn btn-secondary btn-sm" style="font-size: 0.8rem; padding: 0.5rem;" />
          </div>
        </div>
      </div>

      <!-- Forgot Password Form (Initially Hidden) -->
      <div id="forgotFields" style="display: none;">
        <div style="margin-bottom: 1.5rem;">
          <h3 style="color: white; margin: 0 0 0.5rem 0; font-size: 1.25rem; font-weight: 700;">Reset Password</h3>
          <p style="color: var(--text-secondary); font-size: 0.88rem; margin: 0; line-height: 1.4;">Enter your email address below. If an account is found, we will send you a temporary password to log in.</p>
        </div>

        <div id="resetAlert" style="display: none; border-radius: var(--radius-sm); margin-bottom: 1.25rem; padding: 0.85rem 1rem; font-size: 0.9rem; font-weight: 600;">
          <span id="resetAlertText"></span>
        </div>

        <div class="form-group">
          <label>Email Address</label>
          <input type="email" id="txtResetEmail" class="form-input" placeholder="candidate@example.com" />
        </div>

        <button type="button" id="btnResetSubmit" onclick="handleForgotPasswordSubmit()" class="btn btn-primary btn-auth-submit" style="width: 100%; display: flex; align-items: center; justify-content: center; gap: 0.5rem;">
          <span>Send Recovery Email ✉️</span>
        </button>

        <div style="text-align: center; margin-top: 1.5rem;">
          <a href="javascript:void(0)" onclick="toggleForgotPassword(false)" style="color: var(--text-secondary); font-weight: 700; text-decoration: none; font-size: 0.88rem; display: flex; align-items: center; justify-content: center; gap: 0.4rem; transition: color 0.2s;" onmouseover="this.style.color='white'" onmouseout="this.style.color='var(--text-secondary)'">
            <span>← Back to Sign In</span>
          </a>
        </div>
      </div>
    </div>
  </div>

  <!-- EmailJS Integration and Client Script Handles -->
  <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/@emailjs/browser@4/dist/email.min.js"></script>
  <script type="text/javascript">
    (function() {
      emailjs.init({
        publicKey: "7f6GWPVgA3ok7tUsF",
      });
    })();

    function toggleForgotPassword(show) {
      const loginFields = document.getElementById('loginFields');
      const forgotFields = document.getElementById('forgotFields');
      const errorPanel = document.querySelector('[id$="pnlError"]');
      
      if (errorPanel) {
        errorPanel.style.display = 'none';
      }

      if (show) {
        loginFields.style.display = 'none';
        forgotFields.style.display = 'block';
        
        const emailTxt = document.querySelector('input[id$="txtEmail"]');
        if (emailTxt) {
          document.getElementById('txtResetEmail').value = emailTxt.value;
        }
        clearAlert();
      } else {
        loginFields.style.display = 'block';
        forgotFields.style.display = 'none';
      }
    }

    function showAlert(message, isSuccess) {
      const alertDiv = document.getElementById('resetAlert');
      const alertText = document.getElementById('resetAlertText');
      alertDiv.style.display = 'block';
      alertText.innerText = (isSuccess ? '✅ ' : '⚠️ ') + message;
      
      if (isSuccess) {
        alertDiv.style.border = '1px solid #10b981';
        alertDiv.style.background = 'rgba(16, 185, 129, 0.15)';
        alertDiv.style.color = '#10b981';
      } else {
        alertDiv.style.border = '1px solid #ef4444';
        alertDiv.style.background = 'rgba(239, 68, 68, 0.15)';
        alertDiv.style.color = '#ef4444';
      }
    }

    function clearAlert() {
      const alertDiv = document.getElementById('resetAlert');
      alertDiv.style.display = 'none';
    }

    function handleForgotPasswordSubmit() {
      const emailInput = document.getElementById('txtResetEmail');
      const email = emailInput.value.trim();
      const btn = document.getElementById('btnResetSubmit');
      
      if (!email) {
        showAlert('Please enter your email address.', false);
        return;
      }

      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(email)) {
        showAlert('Please enter a valid email address.', false);
        return;
      }

      const originalText = btn.innerHTML;
      btn.disabled = true;
      btn.innerHTML = '<span class="spinner" style="display:inline-block; width:14px; height:14px; border:2px solid rgba(255,255,255,0.3); border-radius:50%; border-top-color:#fff; animation:spin 1s ease-in-out infinite; margin-right:6px;"></span> Sending...';

      fetch('Login.aspx/ResetPassword', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json; charset=utf-8'
        },
        body: JSON.stringify({ email: email })
      })
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        return response.json();
      })
      .then(data => {
        const result = data.d;
        if (result && result.success) {
          const serviceId = 'service_drivelingo';
          const templateId = '7f6GWPVgA3ok7tUsF';
          
          const templateParams = {
            to_email: result.email,
            to_name: result.username,
            temp_password: result.tempPassword,
            message: 'Your temporary password is: ' + result.tempPassword
          };

          return emailjs.send(serviceId, templateId, templateParams)
            .then(function(response) {
              showAlert('A temporary password has been successfully sent to your email address.', true);
              emailInput.value = '';
              const emailTxt = document.querySelector('input[id$="txtEmail"]');
              if (emailTxt) {
                emailTxt.value = result.email;
              }
            }, function(error) {
              console.error('EmailJS Error:', error);
              showAlert('Failed to dispatch recovery email via EmailJS. Password updated in database, but email failed to send. Please contact support.', false);
            });
        } else {
          showAlert(result ? result.message : 'An error occurred. Please try again.', false);
        }
      })
      .catch(err => {
        console.error('Fetch Error:', err);
        showAlert('An error occurred while connecting to the server. Please try again.', false);
      })
      .finally(() => {
        btn.disabled = false;
        btn.innerHTML = originalText;
      });
    }
  </script>
  
  <style>
    @keyframes spin {
      to { transform: rotate(360deg); }
    }
  </style>
</asp:Content>
