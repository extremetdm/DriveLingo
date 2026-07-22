using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DriveLingo.Data;
using DriveLingo.Models;

namespace DriveLingo
{
    public partial class Admin : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User user = Session["CurrentUser"] as User;
            if (user == null || user.Role != "admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindUserGrid();
                BindMetrics();
                BindMaterials();
                BindStore();
                BindAchievements();

                string tab = Request.QueryString["tab"];
                SwitchTab(tab);
            }
        }

        private void SwitchTab(string tab)
        {
            pnlDashboard.Visible = (tab == "dashboard" || string.IsNullOrEmpty(tab));
            pnlUsers.Visible = (tab == "users");
            pnlMaterials.Visible = (tab == "materials");
            pnlSimulation.Visible = (tab == "simulation");
            pnlStore.Visible = (tab == "store");
            pnlAchievements.Visible = (tab == "achievements");
        }

        private void BindUserGrid()
        {
            var repo = AppStateRepository.GetCurrent();
            gvUsers.DataSource = repo.Users;
            gvUsers.DataBind();
        }

        private void BindMetrics()
        {
            var repo = AppStateRepository.GetCurrent();
            litTotalUsers.Text = repo.Users.Count.ToString();
            litTotalAttempts.Text = repo.Attempts.Count.ToString();

            int qCount = 0;
            foreach (var quiz in repo.Quizzes)
            {
                qCount += quiz.Questions.Count;
            }
            litTotalQuestions.Text = qCount.ToString();
        }

        private void BindMaterials()
        {
            var repo = AppStateRepository.GetCurrent();
            gvMaterials.DataSource = repo.Materials;
            gvMaterials.DataBind();
        }

        private void BindStore()
        {
            var repo = AppStateRepository.GetCurrent();
            gvStore.DataSource = repo.StoreItems;
            gvStore.DataBind();
        }

        private void BindAchievements()
        {
            var repo = AppStateRepository.GetCurrent();
            gvAchievements.DataSource = repo.Achievements;
            gvAchievements.DataBind();
        }

        // --- User CRUD Handlers ---
        protected void btnAddUserSubmit_Click(object sender, EventArgs e)
        {
            string name = txtNewUserName.Text.Trim();
            string email = txtNewUserEmail.Text.Trim();
            string password = txtNewUserPassword.Text.Trim();
            string role = ddlNewUserRole.SelectedValue;
            int points = 100;
            int.TryParse(txtNewUserPoints.Text.Trim(), out points);
            int level = 1;
            int.TryParse(txtNewUserLevel.Text.Trim(), out level);

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowNotification("Please provide name, email, and password to create user account.");
                return;
            }

            var repo = AppStateRepository.GetCurrent();
            if (repo.Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                ShowNotification("An account with this email address already exists.");
                return;
            }

            var newUser = new User
            {
                Id = "usr_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                Name = name,
                Email = email,
                Password = password,
                Role = role,
                Points = points,
                Level = level,
                XP = 0,
                Avatar = (role == "educator" ? "👨‍✈️" : role == "admin" ? "👑" : "🚗"),
                Achievements = new List<string>(),
                Inventory = new List<string>(),
                JoinedDate = DateTime.Now.ToString("yyyy-MM-dd")
            };

            repo.Users.Add(newUser);
            ShowNotification("New user account created for " + name + " (" + role.ToUpper() + ")!");

            txtNewUserName.Text = "";
            txtNewUserEmail.Text = "";
            txtNewUserPassword.Text = "";

            BindUserGrid();
            BindMetrics();
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            BindUserGrid();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            BindUserGrid();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string userId = gvUsers.DataKeys[e.RowIndex].Value.ToString();
            GridViewRow row = gvUsers.Rows[e.RowIndex];

            TextBox txtName = row.Cells[1].Controls[0] as TextBox;
            TextBox txtEmail = row.Cells[2].Controls[0] as TextBox;
            DropDownList ddlRole = row.FindControl("ddlEditRole") as DropDownList;
            TextBox txtPoints = row.Cells[4].Controls[0] as TextBox;
            TextBox txtLevel = row.Cells[5].Controls[0] as TextBox;

            var repo = AppStateRepository.GetCurrent();
            var user = repo.Users.FirstOrDefault(u => u.Id == userId);
            User currentUser = Session["CurrentUser"] as User;

            if (user != null && txtName != null && txtEmail != null && ddlRole != null)
            {
                user.Name = txtName.Text.Trim();
                user.Email = txtEmail.Text.Trim();

                // Self-lockout prevention for active logged-in admin
                if (currentUser != null && user.Id == currentUser.Id && ddlRole.SelectedValue != "admin")
                {
                    ShowNotification("You cannot downgrade your own active administrator account role.");
                    user.Role = "admin";
                }
                else
                {
                    user.Role = ddlRole.SelectedValue;
                }

                int pts;
                if (int.TryParse(txtPoints.Text.Trim(), out pts)) user.Points = pts;

                int lvl;
                if (int.TryParse(txtLevel.Text.Trim(), out lvl)) user.Level = lvl;

                gvUsers.EditIndex = -1;
                ShowNotification("User account details for " + user.Name + " updated successfully!");
                BindUserGrid();
            }
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string userId = gvUsers.DataKeys[e.RowIndex].Value.ToString();
            var repo = AppStateRepository.GetCurrent();
            User currentUser = Session["CurrentUser"] as User;

            if (currentUser != null && userId == currentUser.Id)
            {
                ShowNotification("You cannot delete your own active administrator account.");
                return;
            }

            var user = repo.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                repo.Users.Remove(user);
                ShowNotification("User account " + user.Name + " deleted.");
                BindUserGrid();
                BindMetrics();
            }
        }

        // --- Material CRUD Handlers ---
        protected void btnAddMaterial_Click(object sender, EventArgs e)
        {
            string title = txtMatTitle.Text.Trim();
            string category = ddlMatCategory.SelectedValue;
            string pdfUrl = txtMatPdf.Text.Trim();
            string content = txtMatContent.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                ShowNotification("Please provide material title and content.");
                return;
            }

            var repo = AppStateRepository.GetCurrent();
            repo.Materials.Add(new Material
            {
                Id = "mat_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                Title = title,
                Category = category,
                ReadTime = "5 min",
                PdfUrl = pdfUrl,
                Content = content
            });

            txtMatTitle.Text = "";
            txtMatPdf.Text = "";
            txtMatContent.Text = "";
            ShowNotification("New study material guide added!");
            BindMaterials();
        }

        protected void gvMaterials_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string matId = gvMaterials.DataKeys[e.RowIndex].Value.ToString();
            var repo = AppStateRepository.GetCurrent();
            var mat = repo.Materials.FirstOrDefault(m => m.Id == matId);

            if (mat != null)
            {
                repo.Materials.Remove(mat);
                ShowNotification("Study material guide deleted.");
                BindMaterials();
            }
        }

        // --- Simulation Parameter Handlers ---
        protected void btnSaveSimSettings_Click(object sender, EventArgs e)
        {
            ShowNotification("Exam Simulation parameters updated successfully!");
        }

        // --- Store CRUD Handlers ---
        protected void btnAddStoreItem_Click(object sender, EventArgs e)
        {
            string title = txtStoreTitle.Text.Trim();
            string icon = txtStoreIcon.Text.Trim();
            string desc = txtStoreDesc.Text.Trim();
            int price = 100;
            int.TryParse(txtStorePrice.Text.Trim(), out price);

            if (string.IsNullOrEmpty(title))
            {
                ShowNotification("Please enter item title.");
                return;
            }

            var repo = AppStateRepository.GetCurrent();
            repo.StoreItems.Add(new StoreItem
            {
                Id = "item_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                Title = title,
                Icon = string.IsNullOrEmpty(icon) ? "✨" : icon,
                Description = desc,
                Price = price,
                Category = "Cosmetics"
            });

            txtStoreTitle.Text = "";
            txtStoreDesc.Text = "";
            ShowNotification("New store item added!");
            BindStore();
        }

        protected void gvStore_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string itemId = gvStore.DataKeys[e.RowIndex].Value.ToString();
            var repo = AppStateRepository.GetCurrent();
            var item = repo.StoreItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                repo.StoreItems.Remove(item);
                ShowNotification("Store item deleted.");
                BindStore();
            }
        }

        // --- Achievements CRUD Handlers ---
        protected void btnAddAch_Click(object sender, EventArgs e)
        {
            string title = txtAchTitle.Text.Trim();
            string icon = txtAchIcon.Text.Trim();
            string desc = txtAchDesc.Text.Trim();
            int xp = 50;
            int.TryParse(txtAchXp.Text.Trim(), out xp);

            if (string.IsNullOrEmpty(title))
            {
                ShowNotification("Please enter achievement title.");
                return;
            }

            var repo = AppStateRepository.GetCurrent();
            repo.Achievements.Add(new Achievement
            {
                Id = "ach_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                Title = title,
                Icon = string.IsNullOrEmpty(icon) ? "🏆" : icon,
                Description = desc,
                XpBonus = xp
            });

            txtAchTitle.Text = "";
            txtAchDesc.Text = "";
            ShowNotification("New achievement added!");
            BindAchievements();
        }

        protected void gvAchievements_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string achId = gvAchievements.DataKeys[e.RowIndex].Value.ToString();
            var repo = AppStateRepository.GetCurrent();
            var ach = repo.Achievements.FirstOrDefault(a => a.Id == achId);

            if (ach != null)
            {
                repo.Achievements.Remove(ach);
                ShowNotification("Achievement deleted.");
                BindAchievements();
            }
        }

        protected void btnResetState_Click(object sender, EventArgs e)
        {
            Session["AppState"] = null;
            AppStateRepository.GetCurrent();
            BindUserGrid();
            BindMetrics();
            BindMaterials();
            BindStore();
            BindAchievements();
            ShowNotification("Application state and demo data successfully re-seeded.");
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}
