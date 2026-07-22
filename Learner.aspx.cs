using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DriveLingo.Data;
using DriveLingo.Models;

namespace DriveLingo
{
    public partial class Learner : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User user = Session["CurrentUser"] as User;
            if (user == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindDashboardData(user);
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

        private void BindDashboardData(User user)
        {
            var repo = AppStateRepository.GetCurrent();
            // Requirement 4: recent quiz attempts display latest history from top
            var userAttempts = repo.Attempts.Where(a => a.UserId == user.Id).AsEnumerable().Reverse().ToList();

            litLevel.Text = user.Level.ToString();
            litPoints.Text = user.Points.ToString();

            if (userAttempts.Count > 0)
            {
                int passedCount = userAttempts.Count(a => a.Passed);
                int rate = (int)Math.Round((double)passedCount / userAttempts.Count * 100);
                litPassRate.Text = rate + "%";
            }
            else
            {
                litPassRate.Text = "100%";
            }

            gvAttempts.DataSource = userAttempts;
            gvAttempts.DataBind();
        }

        private void BindQuizzes()
        {
            var repo = AppStateRepository.GetCurrent();
            rptQuizzes.DataSource = repo.Quizzes;
            rptQuizzes.DataBind();
        }

        protected void rptQuizzes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Quiz quiz = (Quiz)e.Item.DataItem;
                PlaceHolder phQuizCompletedBadge = (PlaceHolder)e.Item.FindControl("phQuizCompletedBadge");
                User currentUser = Session["CurrentUser"] as User;

                if (quiz != null && currentUser != null && phQuizCompletedBadge != null)
                {
                    if (currentUser.CompletedQuizzes != null && currentUser.CompletedQuizzes.Contains(quiz.Id))
                    {
                        phQuizCompletedBadge.Visible = true;
                    }
                }
            }
        }

        private void BindMaterials()
        {
            var repo = AppStateRepository.GetCurrent();
            rptMaterials.DataSource = repo.Materials;
            rptMaterials.DataBind();
        }

        private void BindStore()
        {
            var repo = AppStateRepository.GetCurrent();
            rptStore.DataSource = repo.StoreItems;
            rptStore.DataBind();
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
                    Session["IsSimulationMode"] = false;
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
            User currentUser = Session["CurrentUser"] as User;

            if (activeQuiz == null || currentUser == null) return;

            bool isSim = Session["IsSimulationMode"] != null && (bool)Session["IsSimulationMode"];

            int correctCount = 0;
            int total = activeQuiz.Questions.Count;

            int cbScore = 0, aScore = 0, bScore = 0, cScore = 0;

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
                            if (isSim)
                            {
                                if (question.Section == "ColorBlindness") cbScore++;
                                else if (question.Section == "SectionA") aScore++;
                                else if (question.Section == "SectionB") bScore++;
                                else if (question.Section == "SectionC") cScore++;
                            }
                        }
                    }
                }
            }

            int percentage = total > 0 ? (int)Math.Round((double)correctCount / total * 100) : 0;
            bool passed = false;

            int awardedPoints = 0;
            if (isSim)
            {
                // Sectional pass criteria:
                // Color Blindness: 8/8 (100%)
                // Section A: 17/21
                // Section B: 28/35
                // Section C: 11/14
                // Overall score: >= 80% (>= 63/78)
                bool cbPassed = (cbScore == 8);
                bool aPassed = (aScore >= 17);
                bool bPassed = (bScore >= 28);
                bool cPassed = (cScore >= 11);

                passed = (percentage >= 80) && cbPassed && aPassed && bPassed && cPassed;
                awardedPoints = 0; // No points for simulation

                // Populate Simulation Breakdown UI
                pnlSimBreakdown.Visible = true;
                litSimCbScore.Text = cbScore + " / 8";
                lblSimCbStatus.Text = cbPassed ? "PASS 🟢" : "FAIL 🔴";
                lblSimCbStatus.CssClass = cbPassed ? "badge badge-success" : "badge badge-danger";

                litSimSecAScore.Text = aScore + " / 21";
                lblSimSecAStatus.Text = aPassed ? "PASS 🟢" : "FAIL 🔴";
                lblSimSecAStatus.CssClass = aPassed ? "badge badge-success" : "badge badge-danger";

                litSimSecBScore.Text = bScore + " / 35";
                lblSimSecBStatus.Text = bPassed ? "PASS 🟢" : "FAIL 🔴";
                lblSimSecBStatus.CssClass = bPassed ? "badge badge-success" : "badge badge-danger";

                litSimSecCScore.Text = cScore + " / 14";
                lblSimSecCStatus.Text = cPassed ? "PASS 🟢" : "FAIL 🔴";
                lblSimSecCStatus.CssClass = cPassed ? "badge badge-success" : "badge badge-danger";
            }
            else
            {
                pnlSimBreakdown.Visible = false;
                passed = percentage >= 70;

                if (passed)
                {
                    if (currentUser.CompletedQuizzes == null)
                    {
                        currentUser.CompletedQuizzes = new List<string>();
                    }

                    bool newlyCompleted = !currentUser.CompletedQuizzes.Contains(activeQuiz.Id);
                    if (newlyCompleted)
                    {
                        currentUser.CompletedQuizzes.Add(activeQuiz.Id);
                        awardedPoints = activeQuiz.RewardPoints;
                    }
                    else
                    {
                        awardedPoints = 0;
                    }
                }
                else
                {
                    awardedPoints = 0;
                }
            }

            int awardedXP = isSim ? (correctCount * 10) : (correctCount * 50);

            currentUser.Points += awardedPoints;
            int oldLevel = currentUser.Level;
            currentUser.XP += awardedXP;
            currentUser.Level = 1 + (currentUser.XP / 200);

            var attempt = new QuizAttempt
            {
                Id = "att_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                UserId = currentUser.Id,
                QuizId = activeQuiz.Id,
                QuizTitle = isSim ? "Official JPJ Exam Simulation (78 Qs)" : activeQuiz.Title,
                Score = correctCount,
                TotalQuestions = total,
                Percentage = percentage,
                Passed = passed,
                DateTaken = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                IsSimulation = isSim,
                ColorBlindScore = cbScore,
                SectionAScore = aScore,
                SectionBScore = bScore,
                SectionCScore = cScore
            };

            var repo = AppStateRepository.GetCurrent();
            repo.Attempts.Add(attempt);

            litResultIcon.Text = passed ? "🎉" : "⚠️";
            litResultHeader.Text = isSim ? (passed ? "Official JPJ Simulation PASSED!" : "Official JPJ Simulation FAILED") : (passed ? "Exam Passed!" : "Exam Needs Improvement");
            litResultScore.Text = correctCount + " / " + total;
            litResultPercentage.Text = percentage.ToString();
            litAwardedPoints.Text = awardedPoints.ToString();
            litAwardedXP.Text = awardedXP.ToString();

            if (isSim)
            {
                if (passed)
                    ShowNotification("Congratulations! You PASSED the Official JPJ 78-Question Simulation Exam (+ " + awardedXP + " XP)");
                else
                    ShowNotification("Simulation Exam Result: FAILED. Please review the sectional criteria failure reasons below.");
            }
            else if (awardedPoints > 0)
            {
                ShowNotification("Quiz Passed! You earned +" + awardedPoints + " Points & +" + awardedXP + " XP!");
            }
            else if (passed)
            {
                ShowNotification("Quiz Passed! +" + awardedXP + " XP earned. (Points bonus was previously claimed for this quiz)");
            }

            Session["IsSimulationMode"] = false;
            pnlActiveExam.Visible = false;
            pnlExamResult.Visible = true;

            BindDashboardData(currentUser);
            BindQuizzes();
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
        protected void rptMaterials_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Material mat = (Material)e.Item.DataItem;
                PlaceHolder phReadBadge = (PlaceHolder)e.Item.FindControl("phReadBadge");
                User currentUser = Session["CurrentUser"] as User;

                if (mat != null && currentUser != null && phReadBadge != null)
                {
                    if (currentUser.ReadMaterials != null && currentUser.ReadMaterials.Contains(mat.Id))
                    {
                        phReadBadge.Visible = true;
                    }
                }
            }
        }

        protected void rptMaterials_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ReadMaterial")
            {
                string materialId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();
                var mat = repo.Materials.FirstOrDefault(m => m.Id == materialId);
                User currentUser = Session["CurrentUser"] as User;

                if (mat != null && currentUser != null)
                {
                    if (currentUser.ReadMaterials == null)
                    {
                        currentUser.ReadMaterials = new List<string>();
                    }

                    bool newlyRead = !currentUser.ReadMaterials.Contains(mat.Id);

                    if (newlyRead)
                    {
                        currentUser.ReadMaterials.Add(mat.Id);
                        int oldLevel = currentUser.Level;

                        currentUser.XP += 15;
                        // Level formula: 1 + (XP / 200)
                        currentUser.Level = 1 + (currentUser.XP / 200);

                        if (currentUser.Level > oldLevel)
                        {
                            ShowNotification("🎉 Level Up! You reached Level " + currentUser.Level + "! (+15 XP for studying " + mat.Title + ")");
                        }
                        else
                        {
                            ShowNotification("Reading guide logged! You earned +15 XP for studying " + mat.Title + ".");
                        }

                        litMatXpStatus.Text = "+15 XP Earned for completing this guide!";
                    }
                    else
                    {
                        ShowNotification("Viewing study guide: " + mat.Title);
                        litMatXpStatus.Text = "✔ Guide Completed (XP bonus already claimed)";
                    }

                    // Populate expanded detail view
                    litMatTitle.Text = mat.Title;
                    litMatCategory.Text = mat.Category;
                    litMatReadTime.Text = mat.ReadTime;
                    litMatContent.Text = mat.Content;

                    if (!string.IsNullOrEmpty(mat.ImageUrl))
                    {
                        imgMatDetail.ImageUrl = mat.ImageUrl;
                        phMatImage.Visible = true;
                    }
                    else
                    {
                        phMatImage.Visible = false;
                    }

                    if (!string.IsNullOrEmpty(mat.PdfUrl))
                    {
                        hlMatPdf.NavigateUrl = mat.PdfUrl;
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
                    BindDashboardData(currentUser);
                    ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
                }
            }
        }

        protected void btnCloseMaterialDetail_Click(object sender, EventArgs e)
        {
            pnlMaterialList.Visible = true;
            pnlMaterialDetail.Visible = false;
        }

        // --- Store Handlers ---
        protected void rptStore_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "BuyItem")
            {
                string itemId = e.CommandArgument.ToString();
                var repo = AppStateRepository.GetCurrent();
                var item = repo.StoreItems.FirstOrDefault(i => i.Id == itemId);
                User currentUser = Session["CurrentUser"] as User;

                if (item != null && currentUser != null)
                {
                    if (currentUser.Inventory.Contains(item.Title))
                    {
                        ShowNotification("You already own " + item.Title + "!");
                        return;
                    }

                    if (currentUser.Points >= item.Price)
                    {
                        currentUser.Points -= item.Price;
                        currentUser.Inventory.Add(item.Title);
                        ShowNotification("Successfully redeemed: " + item.Title + "! Check your profile inventory.");
                        BindDashboardData(currentUser);
                        BindStore();
                        ((SiteMaster)Master).UpdateUserHeaderAndNavigation();
                    }
                    else
                    {
                        ShowNotification("Insufficient points to purchase this item.");
                    }
                }
            }
        }

        protected void rptStore_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                StoreItem item = (StoreItem)e.Item.DataItem;
                User currentUser = Session["CurrentUser"] as User;
                Button btnBuyItem = (Button)e.Item.FindControl("btnBuyItem");
                Label lblOwnedItem = (Label)e.Item.FindControl("lblOwnedItem");

                if (item != null && currentUser != null && btnBuyItem != null && lblOwnedItem != null)
                {
                    bool isOwned = currentUser.Inventory.Contains(item.Title);
                    if (isOwned)
                    {
                        btnBuyItem.Visible = false;
                        lblOwnedItem.Visible = true;
                    }
                    else
                    {
                        btnBuyItem.Visible = true;
                        lblOwnedItem.Visible = false;
                    }
                }
            }
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
            var simQuestions = DriveLingo.Data.SimulationQuestionBank.SampleSimulationQuestions(repo.SimulationQuestions);
            var simQuiz = new Quiz
            {
                Id = "sim_full_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                Title = "Official JPJ KPP01 Full Simulation Test",
                Category = "Simulation",
                RewardPoints = 0,
                Questions = simQuestions
            };

            Session["ActiveQuiz"] = simQuiz;
            Session["ActiveSimQuestions"] = simQuestions;
            Session["IsSimulationMode"] = true;
            litExamTitle.Text = "🏎️ Official JPJ KPP01 Full Simulation Test (78 Questions | 75 Minutes)";
            rptQuestions.DataSource = simQuestions;
            rptQuestions.DataBind();

            SwitchTab("exam");
            pnlQuizList.Visible = false;
            pnlActiveExam.Visible = true;
            pnlExamResult.Visible = false;
        }

        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}
