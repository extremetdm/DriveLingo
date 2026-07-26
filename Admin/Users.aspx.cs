using DriveLingo.Database;
using DriveLingo.Database.Models;
using DriveLingo.Services;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DriveLingo.Admin
{
    public partial class Users : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Admin);

            if (!IsPostBack)
            {
                BindUserGrid();
            }
        }

      
        private void BindUserGrid()
        {
            using (var db = new AppDbContext())
            {
                gvUsers.DataSource = db.Users.ToList();
                gvUsers.DataBind();
            }
        }
        
        // --- User CRUD Handlers ---
        protected void btnAddUserSubmit_Click(object sender, EventArgs e)
        {
            string username = txtNewUserName.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                ShowNotification("Please provide username.");
                return;
            }

            string email = txtNewUserEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                ShowNotification("Please provide email.");
                return;
            }

            string password = txtNewUserPassword.Text.Trim();

            Database.Models.User.UserRole role;
            if (!Enum.TryParse(ddlNewUserRole.SelectedValue, out role))
            {
                ShowNotification("Please provide valid role.");
                return;
            }

            int points = 0;
            int.TryParse(txtNewUserPoints.Text.Trim(), out points);
            int level = 1;
            int.TryParse(txtNewUserLevel.Text.Trim(), out level);

            using (var db = new AppDbContext())
            {
                string editingId = hfEditingUserId.Value;
                Database.Models.User user = null;

                if (!string.IsNullOrEmpty(editingId))
                {
                    user = db.Users.Find(Convert.ToInt32(editingId));
                }

                bool isEdit = user != null;

                var sameEmailQuery = db.Users
                    .Where(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                var sameUsernameQuery = db.Users
                    .Where(u => u.Username == username);

                if (isEdit)
                {
                    // Self-lockout prevention for active logged-in admin
                    if (user.Id == CurrentUser.Id && role != Database.Models.User.UserRole.Admin)
                    {
                        ShowNotification("You cannot downgrade your own active administrator account role.");
                        return;
                    }
                    sameEmailQuery = sameEmailQuery.Where(u => u.Id != user.Id);
                    sameUsernameQuery = sameUsernameQuery.Where(u => u.Id != user.Id);
                }
                else
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        ShowNotification("Please provide password.");
                        return;
                    }
                    user = new Database.Models.User();
                    db.Users.Add(user);
                }


                var sameEmailUser = sameEmailQuery.FirstOrDefault();
                if (sameEmailUser != null)
                {
                    ShowNotification("An account with this email address already exists.");
                    return;
                }
                var sameUsernameUser = sameUsernameQuery.FirstOrDefault();
                if (sameUsernameUser != null)
                {
                    ShowNotification("An account with this username already exists.");
                    return;
                }

                user.Username = username;
                user.Email = email;
                if (!string.IsNullOrEmpty(password))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(password);
                }
                user.Role = role;
                user.Points = points;
                user.XP = LevelingService.TotalRequiredXpForLevel(level);
                user.RegisteredAt = DateTime.Now;

                db.SaveChanges();

                if (isEdit)
                {
                    ShowNotification("User account details for " + user.Username + " updated successfully!");
                } else
                {
                    ShowNotification("New user account created for " + username + " (" + role.ToString().ToUpper() + ")!");
                }

                ResetUserForm();
                BindUserGrid();
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditUser")
            {
                handleEdit(sender, e);
            }
            else if (e.CommandName == "DeleteUser")
            {
                handleDelete(sender, e);
            }
        }

        private void handleEdit(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument.ToString());
            
            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(userId);
                if (user == null)
                {
                    ShowNotification("User not found.");
                    return;
                }

                hfEditingUserId.Value = user.Id.ToString();
                txtNewUserName.Text = user.Username;
                txtNewUserEmail.Text = user.Email;
                txtNewUserPassword.Text = "";
                if (ddlNewUserRole.Items.FindByValue(user.Role.ToString()) != null)
                {
                    ddlNewUserRole.SelectedValue = user.Role.ToString();
                }
                txtNewUserPoints.Text = user.Points.ToString();
                txtNewUserLevel.Text = user.CurrentLevel.ToString();

                litUserFormTitle.Text = "✏️ Edit User Account (" + user.Id + ")";
                btnAddUserSubmit.Text = "💾 Save User Changes";
                btnCancelUserEdit.Visible = true;

                ShowNotification("User " + user.Username + " loaded into editor. Make changes and click 'Save User Changes'.");
            }
        }

        private void handleDelete(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument.ToString());

            if (CurrentUser.Id == userId)
            {
                ShowNotification("You cannot delete your own active administrator account.");
                return;
            }

            using (var db = new AppDbContext())
            {
                var user = db.Users.Find(userId);
                if (user == null)
                {
                    ShowNotification("User not found.");
                    return;
                }
                db.Users.Remove(user);
                db.SaveChanges();
                ShowNotification("User account " + user.Username + " deleted.");
                BindUserGrid();
            }
        }

        protected void btnCancelUserEdit_Click(object sender, EventArgs e)
        {
            ResetUserForm();
            ShowNotification("User edit cancelled.");
        }

        private void ResetUserForm()
        {
            hfEditingUserId.Value = "";
            litUserFormTitle.Text = "➕ Create New User Account";
            btnAddUserSubmit.Text = "➕ Create User Account";
            btnCancelUserEdit.Visible = false;

            txtNewUserName.Text = "";
            txtNewUserEmail.Text = "";
            txtNewUserPassword.Text = "";
            txtNewUserPoints.Text = "0";
            txtNewUserLevel.Text = "1";
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}