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

namespace DriveLingo.Admin
{
    public partial class Lessons : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Admin);

            if (!IsPostBack)
            {
                BindMaterials();
            }
        }

        private void BindMaterials()
        {
            using (var db = new AppDbContext())
            {
                gvMaterials.DataSource = db.Lessons.Select(l => new
                {
                    l.Id,
                    Module = l.Module.Name,
                    l.Title
                }).ToList();
                gvMaterials.DataBind();

                ddlMatCategory.DataSource = db.Modules.ToList();
                ddlMatCategory.DataTextField = "Name";
                ddlMatCategory.DataValueField = "Id";
                ddlMatCategory.DataBind();
            }
        }

        // --- Material CRUD Handlers ---
        protected void btnAddMaterial_Click(object sender, EventArgs e)
        {
            string title = txtMatTitle.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                ShowNotification("Please provide lesson title.");
                return;
            }

            string content = txtMatContent.Text.Trim();
            if (string.IsNullOrEmpty(content))
            {
                ShowNotification("Please provide lesson content.");
                return;
            }

            int estimatedTime;
            if (!int.TryParse(txtEstimatedTime.Text.Trim(), out estimatedTime))
            {
                ShowNotification("Please provide valid reading time.");
                return;
            }

            string pdfUrl = txtMatPdf.Text.Trim();

            using (var db = new AppDbContext())
            {
                int moduleId = Convert.ToInt32(ddlMatCategory.SelectedValue);
                var module = db.Modules.Find(moduleId);
                if (module == null)
                {
                    ShowNotification("Learning Module not found.");
                    return;
                }

                string imageUrl = txtMaterialImageUrl.Text.Trim();
                if (fileMaterialImage.HasFile)
                {
                    var output = UploadService.UploadImage(fileMaterialImage);

                    if (!output.Success)
                    {
                        ShowNotification(output.Message);
                        return;
                    }
                    imageUrl = output.FilePath;
                }

                string lessonId = hfEditingMaterialId.Value;
                Lesson lesson = null;
                if (!string.IsNullOrEmpty(lessonId))
                {
                    lesson = db.Lessons.Find(Convert.ToInt32(lessonId));
                }

                bool isEdit = lesson != null;

                if (!isEdit)
                {
                    lesson = new Lesson();
                    db.Lessons.Add(lesson);
                }

                lesson.Title = title;
                lesson.Module = module;
                lesson.Pdf = pdfUrl;
                lesson.Content = content;
                if (!string.IsNullOrEmpty(imageUrl)) lesson.Image = imageUrl;
                lesson.EstimatedTime = estimatedTime;

                db.SaveChanges();

                if (isEdit)
                {
                    ShowNotification("Study material guide " + lesson.Id + " updated successfully!");
                } else
                {
                    ShowNotification("New study material guide added!");
                }

                ResetMaterialForm();
                BindMaterials();
            }
        }

        protected void gvMaterials_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditMaterial")
            {
                handleEdit(sender, e);
            }
            else if (e.CommandName == "DeleteMaterial")
            {
                handleDelete(sender, e);
            }
        }

        protected void handleEdit(object sender, GridViewCommandEventArgs e)
        {
            int lessonId = Convert.ToInt32(e.CommandArgument.ToString());

            using (var db = new AppDbContext())
            {
                var lesson = db.Lessons.Find(lessonId);
                if (lesson == null)
                {
                    ShowNotification("Lesson material not found.");
                    return;
                }

                hfEditingMaterialId.Value = lesson.Id.ToString();
                txtMatTitle.Text = lesson.Title;
                if (ddlMatCategory.Items.FindByValue(lesson.Module.Id.ToString()) != null)
                {
                    ddlMatCategory.SelectedValue = lesson.Module.Id.ToString();
                }
                txtMaterialImageUrl.Text = lesson.Image;
                txtMatPdf.Text = lesson.Pdf;
                txtMatContent.Text = lesson.Content;
                txtEstimatedTime.Text = lesson.EstimatedTime.ToString();

                litMaterialFormTitle.Text = "✏️ Edit Study Guide Material (" + lesson.Id + ")";
                btnAddMaterial.Text = "💾 Save Material Changes";
                btnCancelMaterialEdit.Visible = true;

                ShowNotification("Material " + lesson.Title + " loaded into editor. Make changes and click 'Save Material Changes'.");
            }
        }

        protected void handleDelete(object sender, GridViewCommandEventArgs e)
        {
            int lessonId = Convert.ToInt32(e.CommandArgument.ToString());

            using (var db = new AppDbContext())
            {
                var lesson = db.Lessons.Find(lessonId);
                if (lesson == null)
                {
                    ShowNotification("Lesson material not found.");
                    return;
                }

                db.Lessons.Remove(lesson);
                db.SaveChanges();

                ShowNotification("Study material guide deleted.");
                BindMaterials();
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
            txtEstimatedTime.Text = "";

            txtMatTitle.Text = "";
            txtMaterialImageUrl.Text = "";
            txtMatPdf.Text = "";
            txtMatContent.Text = "";
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}