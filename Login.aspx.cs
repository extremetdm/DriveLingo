using System;
using System.Linq;
using System.Web.UI;
using DriveLingo.Data;
using DriveLingo.Models;

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

            var repo = AppStateRepository.GetCurrent();
            var user = repo.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password == password);

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
            SetDemoUserAndRedirect("usr_learner", "~/Learner.aspx");
        }

        protected void btnDemoEducator_Click(object sender, EventArgs e)
        {
            SetDemoUserAndRedirect("usr_educator", "~/Educator.aspx");
        }

        protected void btnDemoAdmin_Click(object sender, EventArgs e)
        {
            SetDemoUserAndRedirect("usr_admin", "~/Admin.aspx");
        }

        private void SetDemoUserAndRedirect(string userId, string redirectUrl)
        {
            var repo = AppStateRepository.GetCurrent();
            var user = repo.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                if (userId == "usr_admin") user.Role = "admin";
                else if (userId == "usr_educator") user.Role = "educator";
                else if (userId == "usr_learner") user.Role = "learner";

                Session["CurrentUser"] = user;
                Response.Redirect(redirectUrl);
            }
        }

        private void RedirectUserByRole(string role)
        {
            switch (role.ToLower())
            {
                case "admin":
                    Response.Redirect("~/Admin.aspx");
                    break;
                case "educator":
                    Response.Redirect("~/Educator.aspx");
                    break;
                default:
                    Response.Redirect("~/Learner.aspx");
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
