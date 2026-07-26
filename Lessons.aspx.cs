using DriveLingo.Database;
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
            public bool IsCompleted { get; set; }
        }
        private void BindMaterials()
        {
            using (var db = new AppDbContext())
            {
                int? userId = CurrentUser?.Id;

                rptMaterials.DataSource = db.Lessons
                    .Include(l => l.Module)
                    .Include(l => l.CompletedUsers)
                    .Select(l => new
                    {
                        l.Id,
                        Module = l.Module.Name,
                        l.Title,
                        l.Content,
                        l.Image,
                        l.Pdf,
                        l.EstimatedTime,
                        IsCompleted = l.CompletedUsers.Any(u => u.UserId == userId)
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
                        EstimatedTime = l.EstimatedTime,
                        IsCompleted = l.IsCompleted
                    })
                    .ToList();
                rptMaterials.DataBind();
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
                    ShowNotification("Viewing study guide: " + lesson.Title);

                    if (lesson.CompletedUsers.Any(l => l.UserId == CurrentUser?.Id))
                    {
                        litMatXpStatus.Text = "✔ Guide Completed (XP bonus already claimed)";
                    } else
                    {
                        int xpGain = LevelingService.CalculateXpForLessons(lesson.EstimatedTime ?? 0);
                        litMatXpStatus.Text = $"+{xpGain} XP for completing this guide!";
                    }

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
                    lbCompleteLesson.CommandArgument = lesson.Id.ToString();
                }
            }
        }

        protected void btnCompleteLesson_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName != "CompleteLesson") return;

            if (IsGuest)
            {
                ShowNotification("🔍 Guest Mode: Please register an account to gain XP from lessons!");
                return;
            }

            int lessonId = Convert.ToInt32(e.CommandArgument);

            using (var db = new AppDbContext())
            {
                var lesson = db.Lessons.Find(lessonId);
                if (lesson == null)
                {
                    ShowNotification("Lesson not found.");
                    return;
                }

                var user = db.Users.Find(CurrentUser.Id);
                if (user == null)
                {
                    ShowNotification("User not found.");
                    return;
                }

                var hasLearnt = user.CompletedLessons.Any(ll => ll.LessonId == lesson.Id);

                if (!hasLearnt) {
                    user.CompletedLessons.Add(new Database.Models.CompletedLesson
                    {
                        LessonId = lesson.Id
                    });

                    int xpGain = LevelingService.CalculateXpForLessons(lesson.EstimatedTime ?? 0);
                    user.XP += xpGain;
                    var output = AchievementService.IncrementProgress(db, user, Database.Models.Achievement.TaskType.ReadLessons);

                    if (!output.Success)
                    {
                        ShowNotification(output.Message);
                        return;
                    }

                    db.SaveChanges();
                    ShowNotification($"Reading guide logged! You earned +{xpGain} XP for studying {lesson.Title}.");

                    // Rebind materials to show Read badge on cards
                    BindMaterials();
                    ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
                }
                else
                {
                    pnlNotification.Visible = false;
                }

                pnlMaterialList.Visible = true;
                pnlMaterialDetail.Visible = false;
            }
        }

        protected void btnCloseMaterialDetail_Click(object sender, EventArgs e)
        {
            pnlMaterialList.Visible = true;
            pnlMaterialDetail.Visible = false;
            pnlNotification.Visible = false;
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
            // todo add error msg notification
        }
    }
}