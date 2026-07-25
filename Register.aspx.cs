using System;
using System.Linq;
using System.Web.UI;
using DriveLingo.Data;
using DriveLingo.Models;
using DriveLingo.Services;

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

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowError("Please complete all required fields.");
                return;
            }

            var output = AuthService.Register(name, password, email);

            if (!output.Success)
            {
                ShowError(output.Message);
                return;
            }

            Response.Redirect("~/Dashboard");
        }

        private void ShowError(string message)
        {
            pnlError.Visible = true;
            litErrorMsg.Text = message;
        }
    }
}
