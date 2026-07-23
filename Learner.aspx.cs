using DriveLingo.Data;
using DriveLingo.Models;
using DriveLingo.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using DriveLingo.Services;
using System.Web;

namespace DriveLingo
{
    public partial class Learner : Page
    {
        private Database.Models.User CurrentUser => HttpContext.Current.Items["CurrentUser"] as Database.Models.User;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindDashboardData(CurrentUser);
                BindQuizzes();
                BindMaterials();
                BindStore();
                BindForum();
                BindAchievements();

                string tab = Request.QueryString["tab"];
                if (!string.IsNullOrEmpty(tab))
                {
                    SwitchTab(tab);
                }
            }
        }

        protected void btnTab_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            SwitchTab(btn.CommandArgument);
        }

        public void SwitchTab(string tab)
        {
            pnlDashboard.Visible = (tab == "dashboard" || string.IsNullOrEmpty(tab));
            pnlExam.Visible = (tab == "exam");
            pnlMaterials.Visible = (tab == "materials");
            pnlSimulation.Visible = (tab == "simulation");
            pnlStore.Visible = (tab == "store");
            pnlAchievements.Visible = (tab == "achievements");
            pnlForum.Visible = (tab == "forum");
        }

        struct QuizAttemptSummary
        {
            public string QuizTitle { get; set; }
            public double Percentage { get; set; }
            public int Score { get; set; }
            public bool Passed { get; set; }
            public DateTime CompletedAt { get; set; }

        }

        private void BindDashboardData(Database.Models.User user)
        {
            using (var db = new AppDbContext())
            {
                var attempts = db.QuizAttempts.Where(a => a.UserId == user.Id)
                    .Where(a => a.CompletedAt != null)
                    .Include(a => a.Quiz)
                    .Include(a => a.Answers)
                    .ToList()
                    .Select(a => new QuizAttemptSummary
                    {
                        QuizTitle = a.Quiz.Title,
                        Score = a.Score,
                        Percentage = (double)a.Score / a.Answers.Count,
                        Passed = a.Passed,
                        CompletedAt = a.CompletedAt
                    })
                    .ToList();

                if (attempts.Count > 0)
                {
                    int passedCount = attempts.Count(a => a.Passed);
                    int rate = (int)Math.Round((double)passedCount / attempts.Count * 100);
                    litPassRate.Text = rate + "%";
                }
                else
                {
                    litPassRate.Text = "100%";
                }

                gvAttempts.DataSource = attempts;
                gvAttempts.DataBind();
            }

            litLevel.Text = user.CurrentLevel.ToString();
            litPoints.Text = user.Points.ToString();
        }   

        private void BindQuizzes()
        {
            var repo = AppStateRepository.GetCurrent();
            rptQuizzes.DataSource = repo.Quizzes;
            rptQuizzes.DataBind();
        }

        private void BindMaterials()
        {
            var repo = AppStateRepository.GetCurrent();
            rptMaterials.DataSource = repo.Materials;
            rptMaterials.DataBind();
        }

        private void BindForum()
        {
            var repo = AppStateRepository.GetCurrent();
            rptForum.DataSource = repo.Discussions.OrderByDescending(d => d.DatePosted).ToList();
            rptForum.DataBind();
        }

        // --- Exam Simulator Handlers ---
        protected void rptQuizzes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "StartQuiz")
            {
                string quizId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();
                var quiz = repo.Quizzes.FirstOrDefault(q => q.Id == quizId);

                if (quiz != null)
                {
                    Session["ActiveQuiz"] = quiz;
                    litExamTitle.Text = quiz.Title;
                    rptQuestions.DataSource = quiz.Questions;
                    rptQuestions.DataBind();

                    pnlQuizList.Visible = false;
                    pnlActiveExam.Visible = true;
                    pnlExamResult.Visible = false;
                }
            }
        }

        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Question question = (Question)e.Item.DataItem;
                RadioButtonList rblOptions = (RadioButtonList)e.Item.FindControl("rblOptions");

                if (rblOptions != null && question != null)
                {
                    rblOptions.Items.Clear();
                    for (int i = 0; i < question.Options.Count; i++)
                    {
                        rblOptions.Items.Add(new ListItem(question.Options[i], i.ToString()));
                    }
                }
            }
        }

        protected void btnSubmitExam_Click(object sender, EventArgs e)
        {
            Quiz activeQuiz = Session["ActiveQuiz"] as Quiz;

            if (activeQuiz == null || CurrentUser == null) return;

            int correctCount = 0;
            int total = activeQuiz.Questions.Count;

            foreach (RepeaterItem item in rptQuestions.Items)
            {
                HiddenField hfQuestionId = (HiddenField)item.FindControl("hfQuestionId");
                RadioButtonList rblOptions = (RadioButtonList)item.FindControl("rblOptions");

                if (hfQuestionId != null && rblOptions != null)
                {
                    var question = activeQuiz.Questions.FirstOrDefault(q => q.Id == hfQuestionId.Value);
                    if (question != null && rblOptions.SelectedIndex != -1)
                    {
                        int selectedIndex = Convert.ToInt32(rblOptions.SelectedValue);
                        if (selectedIndex == question.CorrectIndex)
                        {
                            correctCount++;
                        }
                    }
                }
            }

            int percentage = total > 0 ? (int)Math.Round((double)correctCount / total * 100) : 0;
            bool passed = percentage >= 70;

            int awardedPoints = passed ? activeQuiz.RewardPoints : 20;
            int awardedXP = correctCount * 50;

            CurrentUser.Points += awardedPoints;
            CurrentUser.XP += awardedXP;

            var attempt = new QuizAttempt
            {
                Id = "att_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                UserId = CurrentUser.Id.ToString(),
                QuizId = activeQuiz.Id,
                QuizTitle = activeQuiz.Title,
                Score = correctCount,
                TotalQuestions = total,
                Percentage = percentage,
                Passed = passed,
                DateTaken = DateTime.Now.ToString("yyyy-MM-dd")
            };

            var repo = AppStateRepository.GetCurrent();
            repo.Attempts.Add(attempt);

            litResultIcon.Text = passed ? "🎉" : "⚠️";
            litResultHeader.Text = passed ? "Exam Passed!" : "Exam Needs Improvement";
            litResultScore.Text = correctCount + " / " + total;
            litResultPercentage.Text = percentage.ToString();
            litAwardedPoints.Text = awardedPoints.ToString();
            litAwardedXP.Text = awardedXP.ToString();

            pnlActiveExam.Visible = false;
            pnlExamResult.Visible = true;

            BindDashboardData(CurrentUser);
            ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
        }

        protected void btnCancelExam_Click(object sender, EventArgs e)
        {
            pnlQuizList.Visible = true;
            pnlActiveExam.Visible = false;
            pnlExamResult.Visible = false;
        }

        protected void btnBackToQuizzes_Click(object sender, EventArgs e)
        {
            pnlQuizList.Visible = true;
            pnlActiveExam.Visible = false;
            pnlExamResult.Visible = false;
        }

        // --- Material Handlers ---
        protected void rptMaterials_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ReadMaterial")
            {
                ShowNotification("Reading guide logged! You earned +15 XP for studying JPJ guidelines.");
                if (CurrentUser != null)
                {
                    CurrentUser.XP += 15;
                    ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
                }
            }
        }

        // --- Store Handlers ---
        struct ShopAvailableItem
        {
            public int Id { get; set; }
            public string Icon { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Cost { get; set; }
            public bool Owned { get; set; }

        }
        private void BindStore()
        {
            using (var db = new AppDbContext())
            {
                rptStore.DataSource = db.ShopItems
                    .Include(i => i.Redemptions)
                    .ToList()
                    .Select(i => new ShopAvailableItem
                    {
                        Id = i.Id,
                        Icon = i.Icon,
                        Name = i.Name,
                        Description = i.Description,
                        Cost = i.Cost,
                        Owned = i.Redemptions.Any(r => r.UserId == CurrentUser.Id)
                    })
                    .ToList();
                rptStore.DataBind();
            }
        }

        protected void rptStore_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "BuyItem")
            {
                int itemId = (int) e.CommandArgument;

                var output = ShopService.HandleRedeem(CurrentUser.Id, itemId);
                ShowNotification(output.Message);

                if (output.Success)
                {
                    BindDashboardData(CurrentUser);
                    BindStore();
                    ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
                }
            }
        }

        protected void rptStore_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item 
                && e.Item.ItemType != ListItemType.AlternatingItem
            ) return;
            
            var item = (ShopAvailableItem) e.Item.DataItem;
            Button btnBuyItem = (Button)e.Item.FindControl("btnBuyItem");
            Label lblOwnedItem = (Label)e.Item.FindControl("lblOwnedItem");
                
            btnBuyItem.Visible = !item.Owned;
            lblOwnedItem.Visible = item.Owned;
        }

        // --- Forum Handlers ---
        protected void btnToggleNewQuestion_Click(object sender, EventArgs e)
        {
            pnlNewQuestionForm.Visible = !pnlNewQuestionForm.Visible;
        }

        protected void btnPostQuestion_Click(object sender, EventArgs e)
        {
            User currentUser = Session["CurrentUser"] as User;
            if (currentUser == null) return;

            string title = txtForumTitle.Text.Trim();
            string category = ddlForumCategory.SelectedValue;
            string content = txtForumContent.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) return;

            var newThread = new DiscussionThread
            {
                Id = "disc_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                AuthorId = currentUser.Id,
                AuthorName = currentUser.Name,
                AuthorRole = currentUser.Role,
                AuthorAvatar = currentUser.Avatar,
                Title = title,
                Category = category,
                Content = content,
                DatePosted = DateTime.Now.ToString("yyyy-MM-dd"),
                Upvotes = 1
            };

            var repo = AppStateRepository.GetCurrent();
            repo.Discussions.Add(newThread);

            txtForumTitle.Text = "";
            txtForumContent.Text = "";
            pnlNewQuestionForm.Visible = false;

            ShowNotification("Your question has been posted to the community!");
            BindForum();
        }

        protected void rptForum_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Upvote")
            {
                string threadId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();
                var thread = repo.Discussions.FirstOrDefault(d => d.Id == threadId);
                if (thread != null)
                {
                    thread.Upvotes++;
                    BindForum();
                }
            }
            else if (e.CommandName == "ReplyThread")
            {
                string threadId = e.CommandArgument.ToString();
                TextBox txtCandidateReply = (TextBox)e.Item.FindControl("txtCandidateReply");
                User currentUser = Session["CurrentUser"] as User;

                if (txtCandidateReply != null && !string.IsNullOrEmpty(txtCandidateReply.Text.Trim()) && currentUser != null)
                {
                    var repo = AppStateRepository.GetCurrent();
                    var thread = repo.Discussions.FirstOrDefault(d => d.Id == threadId);

                    if (thread != null)
                    {
                        thread.Replies.Add(new DiscussionReply
                        {
                            Id = "rep_" + Guid.NewGuid().ToString("N").Substring(0, 6),
                            AuthorId = currentUser.Id,
                            AuthorName = currentUser.Name,
                            AuthorRole = currentUser.Role,
                            AuthorAvatar = currentUser.Avatar,
                            Content = txtCandidateReply.Text.Trim(),
                            DatePosted = DateTime.Now.ToString("yyyy-MM-dd"),
                            IsEducatorAnswer = false
                        });

                        ShowNotification("Your comment reply has been added!");
                        txtCandidateReply.Text = "";
                        BindForum();
                    }
                }
            }
        }

        protected void rptForum_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DiscussionThread thread = (DiscussionThread)e.Item.DataItem;
                Repeater rptReplies = (Repeater)e.Item.FindControl("rptReplies");
                if (rptReplies != null && thread != null)
                {
                    rptReplies.DataSource = thread.Replies;
                    rptReplies.DataBind();
                }
            }
        }

        private void BindAchievements()
        {
            var repo = AppStateRepository.GetCurrent();
            rptAchievements.DataSource = repo.Achievements;
            rptAchievements.DataBind();
        }

        protected void rptAchievements_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Achievement achievement = (Achievement)e.Item.DataItem;
                User currentUser = Session["CurrentUser"] as User;
                Label lblStatus = (Label)e.Item.FindControl("lblAchievementStatus");

                if (achievement != null && currentUser != null && lblStatus != null)
                {
                    bool isUnlocked = currentUser.Achievements.Contains(achievement.Id);
                    if (isUnlocked)
                    {
                        lblStatus.Text = "Unlocked 🟢 (+" + achievement.XpBonus + " XP)";
                        lblStatus.CssClass = "badge badge-success";
                    }
                    else
                    {
                        lblStatus.Text = "Locked 🔒 (Score threshold required)";
                        lblStatus.CssClass = "badge badge-secondary";
                    }
                }
            }
        }

        protected void btnStartFullSim_Click(object sender, EventArgs e)
        {
            var repo = AppStateRepository.GetCurrent();
            if (repo.Quizzes.Count > 0)
            {
                var quiz = repo.Quizzes[0];
                Session["ActiveQuiz"] = quiz;
                litExamTitle.Text = "Official JPJ KPP01 Simulation Mode";
                rptQuestions.DataSource = quiz.Questions;
                rptQuestions.DataBind();

                SwitchTab("exam");
                pnlQuizList.Visible = false;
                pnlActiveExam.Visible = true;
                pnlExamResult.Visible = false;
            }
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}
