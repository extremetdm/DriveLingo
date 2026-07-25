using DriveLingo.Services;
using System;
using System.Linq;
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
                Session["CurrentUser"] = user;
                RedirectUserByRole(user.Role);
            }
            else
            {
                ShowError("Invalid email address or password.");
            }
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
    }
}
