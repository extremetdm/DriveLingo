using DriveLingo.Services;
using System;
using System.Linq;
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

        [WebMethod]
        public static object ResetPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new { success = false, message = "Please enter an email address." };
            }

            try
            {
                using (var db = new Database.AppDbContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (user == null)
                    {
                        return new { success = false, message = "No account found with this email address." };
                    }

                    // Generate temporary password
                    string tempPassword = GenerateRandomPassword(8);

                    // Update user's password using BCrypt
                    user.Password = BCrypt.Net.BCrypt.HashPassword(tempPassword);
                    db.SaveChanges();

                    return new
                    {
                        success = true,
                        username = user.Username,
                        email = user.Email,
                        tempPassword = tempPassword
                    };
                }
            }
            catch (Exception ex)
            {
                return new { success = false, message = "An error occurred: " + ex.Message };
            }
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
