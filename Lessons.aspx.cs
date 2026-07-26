using DriveLingo.Data;
using DriveLingo.Database;
using DriveLingo.Models;
using DriveLingo.Services;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo
{
    public partial class Lessons : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMaterials();
            }
        }

        struct LearningMaterial
        {
            public int Id { get; set; }
            public string Module { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Image { get; set; }
            public string Pdf { get; set; }
            public int? EstimatedTime { get; set; }
        }
        private void BindMaterials()
        {
            using (var db = new AppDbContext())
            {
                rptMaterials.DataSource = db.Lessons
                    .Include(l => l.Module)
                    .Select(l => new
                    {
                        l.Id,
                        Module = l.Module.Name,
                        l.Title,
                        l.Content,
                        l.Image,
                        l.Pdf,
                        l.EstimatedTime,
                    })
                    .ToList()
                    .Select(l => new LearningMaterial
                    {
                        Id = l.Id,
                        Module = l.Module,
                        Title = l.Title,
                        Content = l.Content,
                        Image = l.Image,
                        Pdf = l.Pdf,
                        EstimatedTime = l.EstimatedTime
                    })
                    .ToList();
                rptMaterials.DataBind();
            }
        }

        // --- Material Handlers ---
        protected void rptMaterials_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LearningMaterial mat = (LearningMaterial)e.Item.DataItem;
                PlaceHolder phReadBadge = (PlaceHolder)e.Item.FindControl("phReadBadge");

                if (phReadBadge != null)
                {
                    //TODO REENABLE THIS
                    //if (currentUser.ReadMaterials != null && currentUser.ReadMaterials.Contains(mat.Id))
                    //{
                    //    phReadBadge.Visible = true;
                    //}
                }
            }
        }

        protected void rptMaterials_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "ReadMaterial") return;

            int lessonId = int.Parse(e.CommandArgument.ToString());

            using (var db = new AppDbContext())
            {
                var lesson = db.Lessons.Find(lessonId);
                if (lesson != null)
                {
                    //if (currentUser.ReadMaterials == null)
                    //{
                    //    currentUser.ReadMaterials = new List<string>();
                    //}

                    //bool newlyRead = !currentUser.ReadMaterials.Contains(mat.Id);

                    //if (newlyRead)
                    //{
                    //    currentUser.ReadMaterials.Add(mat.Id);
                    //    int oldLevel = currentUser.Level;

                    //    currentUser.XP += 15;
                    //    // Level formula: 1 + (XP / 200)
                    //    currentUser.Level = 1 + (currentUser.XP / 200);

                    //    if (currentUser.Level > oldLevel)
                    //    {
                    //        ShowNotification("🎉 Level Up! You reached Level " + currentUser.Level + "! (+15 XP for studying " + mat.Title + ")");
                    //    }
                    //    else
                    //    {
                    //        ShowNotification("Reading guide logged! You earned +15 XP for studying " + mat.Title + ".");
                    //    }

                    //    litMatXpStatus.Text = "+15 XP Earned for completing this guide!";
                    //}
                    //else
                    //{
                        ShowNotification("Viewing study guide: " + lesson.Title);
                    //litMatXpStatus.Text = "✔ Guide Completed (XP bonus already claimed)";
                    //}

                    // Populate expanded detail view
                    litMatTitle.Text = lesson.Title;
                    litMatCategory.Text = lesson.Module.Name;
                    litMatReadTime.Text = lesson.EstimatedTime.ToString() + " min";
                    litMatContent.Text = lesson.Content;

                    if (!string.IsNullOrEmpty(lesson.Image))
                    {
                        imgMatDetail.ImageUrl = lesson.Image;
                        phMatImage.Visible = true;
                    }
                    else
                    {
                        phMatImage.Visible = false;
                    }

                    if (!string.IsNullOrEmpty(lesson.Pdf))
                    {
                        hlMatPdf.NavigateUrl = lesson.Pdf;
                        hlMatPdf.Visible = true;
                    }
                    else
                    {
                        hlMatPdf.Visible = false;
                    }

                    pnlMaterialList.Visible = false;
                    pnlMaterialDetail.Visible = true;

                    // Rebind materials to show Read badge on cards
                    BindMaterials();
                    ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
                }
            }
        }

        protected void btnCloseMaterialDetail_Click(object sender, EventArgs e)
        {
            pnlMaterialList.Visible = true;
            pnlMaterialDetail.Visible = false;
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
            // todo add error msg notification
        }
    }
}