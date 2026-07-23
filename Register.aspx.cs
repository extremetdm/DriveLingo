using System;
using System.Linq;
using System.Web.UI;
using DriveLingo.Data;
using DriveLingo.Models;

namespace DriveLingo
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegisterSubmit_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = "learner";

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowError("Please complete all required fields.");
                return;
            }

            var repo = AppStateRepository.GetCurrent();

            if (repo.Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                ShowError("An account with this email address already exists.");
                return;
            }

            var newUser = new User
            {
                Id = "usr_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                Name = name,
                Email = email,
                Password = password,
                Role = role,
                Avatar = "🚗",
                Points = 100, // Welcome bonus
                Level = 1,
                XP = 0,
                JoinedDate = DateTime.Now.ToString("yyyy-MM-dd")
            };

            repo.Users.Add(newUser);
            Session["CurrentUser"] = newUser;

            Response.Redirect("~/Learner.aspx");
        }

        private void ShowError(string message)
        {
            pnlError.Visible = true;
            litErrorMsg.Text = message;
        }
    }
}
