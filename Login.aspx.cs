using DriveLingo.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.Services;
using System.Web.UI;

namespace DriveLingo
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLoginSubmit_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowError("Please enter both email and password.");
                return;
            }

            var user = AuthService.Login(email, password, true);

            if (user != null)
            {
                RedirectUserByRole(user.Role);
            }
            else
            {
                ShowError("Invalid email address or password.");
            }
        }

        protected void btnContinueGuest_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Dashboard");
        }

        protected void btnDemoLearner_Click(object sender, EventArgs e)
        {
            var user = AuthService.Login("learner@drivelingo.com", "learner", true);
            RedirectUserByRole(user.Role);
        }

        protected void btnDemoEducator_Click(object sender, EventArgs e)
        {
            var user = AuthService.Login("instructor@drivelingo.com", "instructor", true);
            RedirectUserByRole(user.Role);
        }

        protected void btnDemoAdmin_Click(object sender, EventArgs e)
        {
            var user = AuthService.Login("admin@drivelingo.com", "admin", true);
            RedirectUserByRole(user.Role);
        }

        private void RedirectUserByRole(Database.Models.User.UserRole role)
        {
            switch (role)
            {
                case Database.Models.User.UserRole.Admin:
                    Response.Redirect("~/Admin");
                    break;
                case Database.Models.User.UserRole.Instructor:
                    Response.Redirect("~/Instructor");
                    break;
                case Database.Models.User.UserRole.Learner:
                    Response.Redirect("~/Dashboard");
                    break;
            }
        }

        private void ShowError(string message)
        {
            pnlError.Visible = true;
            litErrorMsg.Text = message;
        }

        private void ShowAlert(string message, bool isSuccess)
        {
            loginFields.Style["display"] = "none";
            forgotFields.Style["display"] = "block";
            resetAlert.Visible = true;
            resetAlertText.Text = (isSuccess ? "✅ " : "⚠️ ") + message;

            if (isSuccess)
            {
                resetAlert.Style["border"] = "1px solid #10b981";
                resetAlert.Style["background"] = "rgba(16, 185, 129, 0.15)";
                resetAlert.Style["color"] = "#10b981";
            }
            else
            {
                resetAlert.Style["border"] = "1px solid #ef4444";
                resetAlert.Style["background"] = "rgba(239, 68, 68, 0.15)";
                resetAlert.Style["color"] = "#ef4444";
            }
        }

        protected void btnResetSubmit_Click(object sender, EventArgs e)
        {
            string email = txtResetEmail.Text.Trim();

            // 1. Validation
            if (string.IsNullOrEmpty(email))
            {
                ShowAlert("Please enter your email address.", false);
                return;
            }

            Regex emailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
            if (!emailRegex.IsMatch(email))
            {
                ShowAlert("Please enter a valid email address.", false);
                return;
            }

            //try
            //{
                using (var db = new Database.AppDbContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                    if (user == null)
                    {
                        ShowAlert("No account found with this email address.", false);
                        return;
                    }

                    // Generate temporary password
                    string tempPassword = GenerateRandomPassword(8);

                    // Update user's password using BCrypt
                    user.Password = BCrypt.Net.BCrypt.HashPassword(tempPassword);
                    if (!EmailService.SendPasswordResetEmail(email, tempPassword))
                    {
                        ShowAlert("An error occurred. Please try again.", false);
                        return;
                    }
                    db.SaveChanges();

                    ShowAlert("A temporary password has been successfully sent to your email address.", true);
                }
            //}
            //catch (Exception ex)
            //{
            //    ShowAlert("An error occurred while connecting to the server. Please try again.", false);
            //}
        }


        private static string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            System.Text.StringBuilder res = new System.Text.StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(validChars[rnd.Next(validChars.Length)]);
            }
            return res.ToString();
        }
    }
}
