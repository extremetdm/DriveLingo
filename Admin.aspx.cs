using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DriveLingo.Data;
using DriveLingo.Models;

namespace DriveLingo
{
    public partial class Administrator : Page
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
                BindSimulationQuestions();
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

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                ShowNotification("Please provide name and email.");
                return;
            }

            var repo = AppStateRepository.GetCurrent();
            string editingId = hfEditingUserId.Value;

            if (!string.IsNullOrEmpty(editingId))
            {
                // EDIT EXISTING USER
                var user = repo.Users.FirstOrDefault(u => u.Id == editingId);
                User currentUser = Session["CurrentUser"] as User;

                if (user != null)
                {
                    // Check duplicate email if changed
                    if (!user.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && repo.Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                    {
                        ShowNotification("An account with this email address already exists.");
                        return;
                    }

                    user.Name = name;
                    user.Email = email;
                    if (!string.IsNullOrEmpty(password))
                    {
                        user.Password = password;
                    }

                    // Self-lockout prevention for active logged-in admin
                    if (currentUser != null && user.Id == currentUser.Id && role != "admin")
                    {
                        ShowNotification("You cannot downgrade your own active administrator account role.");
                        user.Role = "admin";
                    }
                    else
                    {
                        user.Role = role;
                    }

                    user.Points = points;
                    user.Level = level;
                    user.Avatar = (user.Role == "educator" ? "👨‍✈️" : user.Role == "admin" ? "👑" : "🚗");

                    ShowNotification("User account details for " + user.Name + " updated successfully!");
                    ResetUserForm();
                    BindUserGrid();
                    BindMetrics();
                }
            }
            else
            {
                // CREATE NEW USER
                if (string.IsNullOrEmpty(password))
                {
                    ShowNotification("Please provide password to create user account.");
                    return;
                }

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

                ResetUserForm();
                BindUserGrid();
                BindMetrics();
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditUser")
            {
                string userId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();
                var user = repo.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    hfEditingUserId.Value = user.Id;
                    txtNewUserName.Text = user.Name;
                    txtNewUserEmail.Text = user.Email;
                    txtNewUserPassword.Text = "";
                    if (ddlNewUserRole.Items.FindByValue(user.Role) != null)
                    {
                        ddlNewUserRole.SelectedValue = user.Role;
                    }
                    txtNewUserPoints.Text = user.Points.ToString();
                    txtNewUserLevel.Text = user.Level.ToString();

                    litUserFormTitle.Text = "✏️ Edit User Account (" + user.Id + ")";
                    btnAddUserSubmit.Text = "💾 Save User Changes";
                    btnCancelUserEdit.Visible = true;

                    ShowNotification("User " + user.Name + " loaded into editor. Make changes and click 'Save User Changes'.");
                }
            }
            else if (e.CommandName == "DeleteUser")
            {
                string userId = e.CommandArgument.ToString();
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
            txtNewUserPoints.Text = "100";
            txtNewUserLevel.Text = "1";
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Handled in gvUsers_RowCommand
        }

        // --- Material CRUD Handlers ---
        protected void btnAddMaterial_Click(object sender, EventArgs e)
        {
            string title = txtMatTitle.Text.Trim();
            string category = ddlMatCategory.SelectedValue;
            string pdfUrl = txtMatPdf.Text.Trim();
            string content = txtMatContent.Text.Trim();
            string imageUrl = txtMaterialImageUrl.Text.Trim();

            if (fileMaterialImage.HasFile)
            {
                try
                {
                    string uploadsDir = Server.MapPath("~/uploads/");
                    if (!System.IO.Directory.Exists(uploadsDir))
                    {
                        System.IO.Directory.CreateDirectory(uploadsDir);
                    }

                    string fileName = "sign_mat_" + Guid.NewGuid().ToString("N").Substring(0, 8) + System.IO.Path.GetExtension(fileMaterialImage.FileName);
                    fileMaterialImage.SaveAs(uploadsDir + fileName);
                    imageUrl = "uploads/" + fileName;
                }
                catch (Exception ex)
                {
                    ShowNotification("Image upload error: " + ex.Message);
                }
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                ShowNotification("Please provide material title and content.");
                return;
            }

            var repo = AppStateRepository.GetCurrent();
            string editingId = hfEditingMaterialId.Value;

            if (!string.IsNullOrEmpty(editingId))
            {
                // EDIT EXISTING MATERIAL
                var mat = repo.Materials.FirstOrDefault(m => m.Id == editingId);
                if (mat != null)
                {
                    mat.Title = title;
                    mat.Category = category;
                    mat.PdfUrl = pdfUrl;
                    mat.Content = content;
                    if (!string.IsNullOrEmpty(imageUrl)) mat.ImageUrl = imageUrl;

                    ShowNotification("Study material guide " + mat.Id + " updated successfully!");
                    ResetMaterialForm();
                    BindMaterials();
                }
            }
            else
            {
                // CREATE NEW MATERIAL
                repo.Materials.Add(new Material
                {
                    Id = "mat_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                    Title = title,
                    Category = category,
                    ReadTime = "5 min",
                    ImageUrl = imageUrl,
                    PdfUrl = pdfUrl,
                    Content = content
                });

                ShowNotification("New study material guide added!");
                ResetMaterialForm();
                BindMaterials();
            }
        }

        protected void gvMaterials_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditMaterial")
            {
                string matId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();
                var mat = repo.Materials.FirstOrDefault(m => m.Id == matId);

                if (mat != null)
                {
                    hfEditingMaterialId.Value = mat.Id;
                    txtMatTitle.Text = mat.Title;
                    if (ddlMatCategory.Items.FindByValue(mat.Category) != null)
                    {
                        ddlMatCategory.SelectedValue = mat.Category;
                    }
                    txtMaterialImageUrl.Text = mat.ImageUrl;
                    txtMatPdf.Text = mat.PdfUrl;
                    txtMatContent.Text = mat.Content;

                    litMaterialFormTitle.Text = "✏️ Edit Study Guide Material (" + mat.Id + ")";
                    btnAddMaterial.Text = "💾 Save Material Changes";
                    btnCancelMaterialEdit.Visible = true;

                    ShowNotification("Material " + mat.Title + " loaded into editor. Make changes and click 'Save Material Changes'.");
                }
            }
            else if (e.CommandName == "DeleteMaterial")
            {
                string matId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();
                var mat = repo.Materials.FirstOrDefault(m => m.Id == matId);

                if (mat != null)
                {
                    repo.Materials.Remove(mat);
                    ShowNotification("Study material guide deleted.");
                    BindMaterials();
                }
            }
        }

        protected void btnCancelMaterialEdit_Click(object sender, EventArgs e)
        {
            ResetMaterialForm();
            ShowNotification("Material edit cancelled.");
        }

        private void ResetMaterialForm()
        {
            hfEditingMaterialId.Value = "";
            litMaterialFormTitle.Text = "➕ Add Study Guide Material";
            btnAddMaterial.Text = "➕ Create Material";
            btnCancelMaterialEdit.Visible = false;

            txtMatTitle.Text = "";
            txtMaterialImageUrl.Text = "";
            txtMatPdf.Text = "";
            txtMatContent.Text = "";
        }

        protected void gvMaterials_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Handled in gvMaterials_RowCommand
        }

        // --- Simulation Question Bank CRUD Handlers ---
        private void BindSimulationQuestions()
        {
            var repo = AppStateRepository.GetCurrent();
            string selectedSec = ddlFilterSimSection.SelectedValue;

            if (selectedSec == "ALL")
            {
                gvSimQuestions.DataSource = repo.SimulationQuestions;
            }
            else
            {
                gvSimQuestions.DataSource = repo.SimulationQuestions.Where(q => q.Section == selectedSec).ToList();
            }
            gvSimQuestions.DataBind();
        }

        protected void ddlFilterSimSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSimulationQuestions();
        }

        protected void btnAddSimQuestion_Click(object sender, EventArgs e)
        {
            string qText = txtSimQuestionText.Text.Trim();
            string opt1 = txtSimOpt1.Text.Trim();
            string opt2 = txtSimOpt2.Text.Trim();
            string opt3 = txtSimOpt3.Text.Trim();
            string opt4 = txtSimOpt4.Text.Trim();

            if (string.IsNullOrEmpty(qText) || string.IsNullOrEmpty(opt1) || string.IsNullOrEmpty(opt2))
            {
                ShowNotification("Please provide question text and at least 2 options.");
                return;
            }

            var repo = AppStateRepository.GetCurrent();
            string editingId = hfEditingSimQuestionId.Value;

            var options = new List<string> { opt1, opt2 };
            if (!string.IsNullOrEmpty(opt3)) options.Add(opt3);
            if (!string.IsNullOrEmpty(opt4)) options.Add(opt4);

            int correctIdx = Convert.ToInt32(ddlSimCorrect.SelectedValue);
            if (correctIdx >= options.Count) correctIdx = 0;

            if (!string.IsNullOrEmpty(editingId))
            {
                // Update Existing Simulation Question
                var existingQ = repo.SimulationQuestions.FirstOrDefault(q => q.Id == editingId);
                if (existingQ != null)
                {
                    existingQ.Section = ddlSimSection.SelectedValue;
                    existingQ.Text = qText;
                    existingQ.Options = options;
                    existingQ.CorrectIndex = correctIdx;
                    existingQ.ImageUrl = txtSimImageUrl.Text.Trim();
                    existingQ.Explanation = txtSimExplanation.Text.Trim();

                    ShowNotification("Simulation Question updated successfully!");
                }
            }
            else
            {
                // Create New Simulation Question
                var newQ = new Question
                {
                    Id = "sim_q_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                    Section = ddlSimSection.SelectedValue,
                    Text = qText,
                    Options = options,
                    CorrectIndex = correctIdx,
                    ImageUrl = txtSimImageUrl.Text.Trim(),
                    Explanation = txtSimExplanation.Text.Trim()
                };
                repo.SimulationQuestions.Add(newQ);
                ShowNotification("New Simulation Question added to bank!");
            }

            ResetSimForm();
            BindSimulationQuestions();
        }

        protected void btnCancelSimEdit_Click(object sender, EventArgs e)
        {
            ResetSimForm();
            ShowNotification("Edit cancelled.");
        }

        protected void gvSimQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string qId = e.CommandArgument.ToString();
            var repo = AppStateRepository.GetCurrent();

            if (e.CommandName == "EditSimQuestion")
            {
                var q = repo.SimulationQuestions.FirstOrDefault(x => x.Id == qId);
                if (q != null)
                {
                    hfEditingSimQuestionId.Value = q.Id;
                    litSimFormTitle.Text = "✏️ Edit Simulation Question";
                    btnAddSimQuestion.Text = "💾 Save Changes";
                    btnCancelSimEdit.Visible = true;

                    if (ddlSimSection.Items.FindByValue(q.Section) != null)
                        ddlSimSection.SelectedValue = q.Section;

                    txtSimQuestionText.Text = q.Text;
                    txtSimOpt1.Text = q.Options.Count > 0 ? q.Options[0] : "";
                    txtSimOpt2.Text = q.Options.Count > 1 ? q.Options[1] : "";
                    txtSimOpt3.Text = q.Options.Count > 2 ? q.Options[2] : "";
                    txtSimOpt4.Text = q.Options.Count > 3 ? q.Options[3] : "";

                    if (ddlSimCorrect.Items.FindByValue(q.CorrectIndex.ToString()) != null)
                        ddlSimCorrect.SelectedValue = q.CorrectIndex.ToString();

                    txtSimImageUrl.Text = q.ImageUrl;
                    txtSimExplanation.Text = q.Explanation;

                    ShowNotification("Loaded simulation question into editor.");
                }
            }
            else if (e.CommandName == "DeleteSimQuestion")
            {
                var q = repo.SimulationQuestions.FirstOrDefault(x => x.Id == qId);
                if (q != null)
                {
                    repo.SimulationQuestions.Remove(q);
                    ResetSimForm();
                    BindSimulationQuestions();
                    ShowNotification("Simulation question deleted from question bank.");
                }
            }
        }

        protected void gvSimQuestions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Handled in gvSimQuestions_RowCommand
        }

        private void ResetSimForm()
        {
            hfEditingSimQuestionId.Value = "";
            litSimFormTitle.Text = "➕ Add Simulation Question";
            btnAddSimQuestion.Text = "➕ Create Simulation Question";
            btnCancelSimEdit.Visible = false;

            txtSimQuestionText.Text = "";
            txtSimOpt1.Text = "";
            txtSimOpt2.Text = "";
            txtSimOpt3.Text = "";
            txtSimOpt4.Text = "";
            txtSimImageUrl.Text = "";
            txtSimExplanation.Text = "";
            ddlSimSection.SelectedIndex = 0;
            ddlSimCorrect.SelectedIndex = 0;
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
            string editingId = hfEditingStoreItemId.Value;

            if (!string.IsNullOrEmpty(editingId))
            {
                // EDIT EXISTING STORE ITEM
                var item = repo.StoreItems.FirstOrDefault(i => i.Id == editingId);
                if (item != null)
                {
                    item.Title = title;
                    item.Icon = string.IsNullOrEmpty(icon) ? "✨" : icon;
                    item.Description = desc;
                    item.Price = price;

                    ShowNotification("Store item " + item.Id + " updated successfully!");
                    ResetStoreForm();
                    BindStore();
                }
            }
            else
            {
                // CREATE NEW STORE ITEM
                repo.StoreItems.Add(new StoreItem
                {
                    Id = "item_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                    Title = title,
                    Icon = string.IsNullOrEmpty(icon) ? "✨" : icon,
                    Description = desc,
                    Price = price,
                    Category = "Cosmetics"
                });

                ShowNotification("New store item added!");
                ResetStoreForm();
                BindStore();
            }
        }

        protected void gvStore_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditStoreItem")
            {
                string itemId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();
                var item = repo.StoreItems.FirstOrDefault(i => i.Id == itemId);

                if (item != null)
                {
                    hfEditingStoreItemId.Value = item.Id;
                    txtStoreTitle.Text = item.Title;
                    txtStoreIcon.Text = item.Icon;
                    txtStorePrice.Text = item.Price.ToString();
                    txtStoreDesc.Text = item.Description;

                    litStoreFormTitle.Text = "✏️ Edit Store Item (" + item.Id + ")";
                    btnAddStoreItem.Text = "💾 Save Store Item Changes";
                    btnCancelStoreEdit.Visible = true;

                    ShowNotification("Store item " + item.Title + " loaded into editor. Make changes and click 'Save Store Item Changes'.");
                }
            }
            else if (e.CommandName == "DeleteStoreItem")
            {
                string itemId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();
                var item = repo.StoreItems.FirstOrDefault(i => i.Id == itemId);

                if (item != null)
                {
                    repo.StoreItems.Remove(item);
                    ShowNotification("Store item deleted.");
                    BindStore();
                }
            }
        }

        protected void btnCancelStoreEdit_Click(object sender, EventArgs e)
        {
            ResetStoreForm();
            ShowNotification("Store item edit cancelled.");
        }

        private void ResetStoreForm()
        {
            hfEditingStoreItemId.Value = "";
            litStoreFormTitle.Text = "➕ Create Store Item";
            btnAddStoreItem.Text = "➕ Create Store Item";
            btnCancelStoreEdit.Visible = false;

            txtStoreTitle.Text = "";
            txtStoreIcon.Text = "✨";
            txtStorePrice.Text = "200";
            txtStoreDesc.Text = "";
        }

        protected void gvStore_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Handled in gvStore_RowCommand
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
            string editingId = hfEditingAchId.Value;

            if (!string.IsNullOrEmpty(editingId))
            {
                // EDIT EXISTING ACHIEVEMENT
                var ach = repo.Achievements.FirstOrDefault(a => a.Id == editingId);
                if (ach != null)
                {
                    ach.Title = title;
                    ach.Icon = string.IsNullOrEmpty(icon) ? "🏆" : icon;
                    ach.Description = desc;
                    ach.XpBonus = xp;

                    ShowNotification("Achievement " + ach.Id + " updated successfully!");
                    ResetAchForm();
                    BindAchievements();
                }
            }
            else
            {
                // CREATE NEW ACHIEVEMENT
                repo.Achievements.Add(new Achievement
                {
                    Id = "ach_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                    Title = title,
                    Icon = string.IsNullOrEmpty(icon) ? "🏆" : icon,
                    Description = desc,
                    XpBonus = xp
                });

                ShowNotification("New achievement added!");
                ResetAchForm();
                BindAchievements();
            }
        }

        protected void gvAchievements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditAchievement")
            {
                string achId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();
                var ach = repo.Achievements.FirstOrDefault(a => a.Id == achId);

                if (ach != null)
                {
                    hfEditingAchId.Value = ach.Id;
                    txtAchTitle.Text = ach.Title;
                    txtAchIcon.Text = ach.Icon;
                    txtAchXp.Text = ach.XpBonus.ToString();
                    txtAchDesc.Text = ach.Description;

                    litAchFormTitle.Text = "✏️ Edit Achievement (" + ach.Id + ")";
                    btnAddAch.Text = "💾 Save Achievement Changes";
                    btnCancelAchEdit.Visible = true;

                    ShowNotification("Achievement " + ach.Title + " loaded into editor. Make changes and click 'Save Achievement Changes'.");
                }
            }
            else if (e.CommandName == "DeleteAchievement")
            {
                string achId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();
                var ach = repo.Achievements.FirstOrDefault(a => a.Id == achId);

                if (ach != null)
                {
                    repo.Achievements.Remove(ach);
                    ShowNotification("Achievement deleted.");
                    BindAchievements();
                }
            }
        }

        protected void btnCancelAchEdit_Click(object sender, EventArgs e)
        {
            ResetAchForm();
            ShowNotification("Achievement edit cancelled.");
        }

        private void ResetAchForm()
        {
            hfEditingAchId.Value = "";
            litAchFormTitle.Text = "➕ Create Achievement";
            btnAddAch.Text = "➕ Create Achievement";
            btnCancelAchEdit.Visible = false;

            txtAchTitle.Text = "";
            txtAchIcon.Text = "🏆";
            txtAchXp.Text = "100";
            txtAchDesc.Text = "";
        }

        protected void gvAchievements_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Handled in gvAchievements_RowCommand
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
