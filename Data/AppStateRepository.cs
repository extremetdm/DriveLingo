using DriveLingo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace DriveLingo.Data
{
    public class AppStateRepository
    {
        public List<User> Users { get; set; }
        public List<ModuleItem> Modules { get; set; }
        public List<Quiz> Quizzes { get; set; }
        public List<Material> Materials { get; set; }
        public List<DiscussionThread> Discussions { get; set; }
        public List<StoreItem> StoreItems { get; set; }
        public List<Achievement> Achievements { get; set; }
        public List<QuizAttempt> Attempts { get; set; }
        public List<Question> SimulationQuestions { get; set; }

        public AppStateRepository()
        {
            Users = new List<User>();
            Modules = new List<ModuleItem>();
            Quizzes = new List<Quiz>();
            Materials = new List<Material>();
            Discussions = new List<DiscussionThread>();
            StoreItems = new List<StoreItem>();
            Achievements = new List<Achievement>();
            Attempts = new List<QuizAttempt>();
            SimulationQuestions = SimulationQuestionBank.GetAllQuestions();

            SeedInitialData();
        }

        public static AppStateRepository GetCurrent()
        {
            if (HttpContext.Current.Session["AppState"] == null)
            {
                var state = new AppStateRepository();
                HttpContext.Current.Session["AppState"] = state;
            }
            return (AppStateRepository)HttpContext.Current.Session["AppState"];
        }

        private void SeedInitialData()
        {
            // Seed Users
            Users.Add(new User
            {
                Id = "usr_admin",
                Email = "admin@drivelingo.com",
                Password = "admin123",
                Role = "admin",
                Name = "Elena Vance (Director)",
                Avatar = "👑",
                Points = 0,
                Level = 1,
                XP = 0,
                JoinedDate = "2026-01-10"
            });

            Users.Add(new User
            {
                Id = "usr_educator",
                Email = "educator@drivelingo.com",
                Password = "educator123",
                Role = "educator",
                Name = "Inspector Ali (JPJ Officer)",
                Avatar = "👨‍✈️",
                Points = 0,
                Level = 5,
                XP = 4500,
                Achievements = new List<string> { "quiz_creator", "verified_expert" },
                JoinedDate = "2026-02-15"
            });

            Users.Add(new User
            {
                Id = "usr_learner",
                Email = "learner@drivelingo.com",
                Password = "learner123",
                Role = "learner",
                Name = "Alex Hero",
                Avatar = "🚗",
                Points = 5000,
                Level = 2,
                XP = 180,
                Achievements = new List<string> { "first_step" },
                Inventory = new List<string> { "Border: Glowing Neon" },
                EquippedBorder = "Border: Glowing Neon",
                JoinedDate = "2026-07-01"
            });

            // Seed Dynamic Modules with Admin-configured Reward Points per Question
            Modules.Add(new ModuleItem { Id = "mod_sec_a", Name = "Section A - Road Signs", Description = "Prohibitory, warning, and mandatory road sign regulations.", Icon = "🛑", RewardPointsPerQuestion = 20 });
            Modules.Add(new ModuleItem { Id = "mod_sec_b", Name = "Section B - Rules of the Road", Description = "Speed limits, lane discipline, traffic signals, and right of way.", Icon = "🚗", RewardPointsPerQuestion = 25 });
            Modules.Add(new ModuleItem { Id = "mod_sec_c", Name = "Section C - KEJARA & Safety", Description = "Demerit point penalties, alcohol laws, and emergency procedures.", Icon = "🚦", RewardPointsPerQuestion = 30 });
            Modules.Add(new ModuleItem { Id = "mod_cb", Name = "Color Blind", Description = "Official Ishihara color vision screening plates.", Icon = "👁️", RewardPointsPerQuestion = 15 });

            // Seed Quizzes & Questions
            var quiz1 = new Quiz
            {
                Id = "quiz_signs_1",
                Title = "JPJ Road Signs Quiz",
                Category = "Road Signs",
                RewardPoints = 100,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Id = "q_s1",
                        QuizId = "quiz_signs_1",
                        Text = "What does the road sign shown below indicate?",
                        ImageUrl = "uploads/no_entry.svg",
                        Options = new List<string> { "No Parking Zone", "No Entry (Dilarang Masuk)", "Stop Command", "Speed Limit Ahead" },
                        CorrectIndex = 1,
                        Explanation = "A circular sign with a red background and horizontal white bar represents the 'No Entry' command."
                    },
                    new Question
                    {
                        Id = "q_s2",
                        QuizId = "quiz_signs_1",
                        Text = "What does this yellow diamond road sign warn drivers about?",
                        ImageUrl = "uploads/warning_curve.svg",
                        Options = new List<string> { "Slippery Road", "Sharp Right Curve Ahead", "Narrow Bridge", "Roundabout Ahead" },
                        CorrectIndex = 1,
                        Explanation = "Yellow diamond signs with black symbol curves warn drivers of an upcoming sharp turn or curve."
                    },
                    new Question
                    {
                        Id = "q_s3",
                        QuizId = "quiz_signs_1",
                        Text = "What type of highway route is indicated by the green background sign below?",
                        ImageUrl = "uploads/expressway_sign.svg",
                        Options = new List<string> { "State Highway", "Malaysian Expressway (Lebuhraya)", "Federal Route", "Town Municipal Road" },
                        CorrectIndex = 1,
                        Explanation = "Green backgrounds are reserved exclusively for Expressways (Lebuhraya). Blue backgrounds represent Federal/State roads."
                    }
                }
            };

            var quiz2 = new Quiz
            {
                Id = "quiz_rules_1",
                Title = "Speed Limits & Regulations Quiz",
                Category = "Rules & Safety",
                RewardPoints = 120,
                Questions = new List<Question>
                {
                    new Question
                    {
                        Id = "q_r1",
                        QuizId = "quiz_rules_1",
                        Text = "What is the maximum legal speed limit represented by this regulatory sign?",
                        ImageUrl = "uploads/speed_limit_110.svg",
                        Options = new List<string> { "90 km/h", "100 km/h", "110 km/h", "120 km/h" },
                        CorrectIndex = 2,
                        Explanation = "The default expressway speed limit is 110 km/h under normal conditions."
                    },
                    new Question
                    {
                        Id = "q_r2",
                        QuizId = "quiz_rules_1",
                        Text = "What is the maximum speed limit in school zones?",
                        Options = new List<string> { "30 km/h", "40 km/h", "50 km/h", "60 km/h" },
                        CorrectIndex = 0,
                        Explanation = "In school zones, speed limits are strictly capped at 30 km/h to protect children."
                    }
                }
            };

            Quizzes.Add(quiz1);
            Quizzes.Add(quiz2);

            // Seed Study Materials
            Materials.Add(new Material
            {
                Id = "mat_rules_1",
                Title = "Malaysian Speed Limits Guidelines",
                Category = "Rules & Safety",
                ReadTime = "5 min",
                ImageUrl = "uploads/speed_limit_110.svg",
                PdfUrl = "https://www.jpj.gov.my/documents/20124/0/KPP+Class+D+Manual.pdf",
                Content = "Overview of Malaysian Speed Limits. Expressways (110 km/h), Federal/State Roads (90 km/h), Municipal/Town (60 km/h), School Zones (30 km/h)."
            });

            Materials.Add(new Material
            {
                Id = "mat_signs_1",
                Title = "Understanding Regulatory vs. Warning Signs",
                Category = "Road Signs",
                ReadTime = "6 min",
                ImageUrl = "uploads/no_entry.svg",
                PdfUrl = "",
                Content = "Regulatory Signs (Circular red border = prohibition / Stop sign = octagonal red), Warning Signs (Diamond yellow background), Informational Signs (Green = Expressway, Blue = Federal)."
            });

            // Seed Store Items (Categories: Border, Icon, Badge)
            StoreItems.Add(new StoreItem { Id = "border_neon", Title = "Glowing Neon Frame", Description = "Cyberpunk neon glowing outline for avatar icon", Price = 150, Icon = "✨", Category = "Border" });
            StoreItems.Add(new StoreItem { Id = "border_gold", Title = "Gold Crown Frame", Description = "Royal gold border outline for profile avatar", Price = 250, Icon = "👑", Category = "Border" });
            StoreItems.Add(new StoreItem { Id = "border_emerald", Title = "Cyber Emerald Frame", Description = "Glowing emerald glass border frame", Price = 300, Icon = "🟢", Category = "Border" });

            StoreItems.Add(new StoreItem { Id = "icon_supercar", Title = "Red Supercar", Description = "High-speed red supercar icon", Price = 200, Icon = "🏎️", Category = "Icon" });
            StoreItems.Add(new StoreItem { Id = "icon_motorbike", Title = "Racing Motorbike", Description = "Sporty motorcycling icon", Price = 180, Icon = "🏍️", Category = "Icon" });
            StoreItems.Add(new StoreItem { Id = "icon_lightning", Title = "Lightning Wheel", Description = "Electric lightning wheel icon", Price = 220, Icon = "⚡", Category = "Icon" });

            StoreItems.Add(new StoreItem { Id = "badge_speed", Title = "Speed Master Badge", Description = "Displays ⚡ Speed Master badge next to your display name", Price = 250, Icon = "⚡", Category = "Badge" });
            StoreItems.Add(new StoreItem { Id = "badge_flawless", Title = "Flawless Driver Badge", Description = "Displays 🏆 Flawless Driver badge next to your display name", Price = 350, Icon = "🏆", Category = "Badge" });
            StoreItems.Add(new StoreItem { Id = "badge_scholar", Title = "Traffic Scholar Badge", Description = "Displays 👨‍✈️ Traffic Scholar badge next to your display name", Price = 300, Icon = "👨‍✈️", Category = "Badge" });

            // Seed Achievements with Target Counts & Metric Types
            Achievements.Add(new Achievement { Id = "first_step", Title = "First Step", Description = "Complete your first JPJ practice quiz", Icon = "🎯", XpBonus = 50, TargetCount = 1, MetricType = "quiz_count" });
            Achievements.Add(new Achievement { Id = "quiz_master", Title = "Theory Master", Description = "Answer 5 practice quizzes to unlock", Icon = "📚", XpBonus = 200, TargetCount = 5, MetricType = "quiz_count" });
            Achievements.Add(new Achievement { Id = "perfect_score", Title = "Flawless Driver", Description = "Score 100% on any practice test", Icon = "🏆", XpBonus = 150, TargetCount = 1, MetricType = "perfect_score" });
            Achievements.Add(new Achievement { Id = "study_bug", Title = "Diligent Scholar", Description = "Read 3 JPJ study guides to unlock", Icon = "📖", XpBonus = 100, TargetCount = 3, MetricType = "materials_read" });

            // Seed Initial Discussion Thread
            Discussions.Add(new DiscussionThread
            {
                Id = "disc_1",
                AuthorId = "usr_learner",
                AuthorName = "Alex Hero",
                AuthorRole = "learner",
                AuthorAvatar = "🚗",
                Title = "What is the penalty for exceeding 110 km/h on PLUS Highway?",
                Category = "Rules & Safety",
                Content = "Hi guys! I want to confirm if summonses for speeding over 110km/h on highway carry demerit points under Kejara system?",
                DatePosted = "2026-07-20",
                Upvotes = 3,
                Replies = new List<DiscussionReply>
                {
                    new DiscussionReply
                    {
                        Id = "rep_1",
                        AuthorId = "usr_educator",
                        AuthorName = "Inspector Ali (JPJ Officer)",
                        AuthorRole = "educator",
                        AuthorAvatar = "👨‍✈️",
                        Content = "Yes! Speeding offenses carry a compound fine up to RM300 and demerit points under the KEJARA system depending on how much you exceeded the limit.",
                        DatePosted = "2026-07-20",
                        IsEducatorAnswer = true
                    }
                }
            });

            // Seed initial attempt
            Attempts.Add(new QuizAttempt
            {
                Id = "att_1",
                UserId = "usr_learner",
                QuizId = "quiz_signs_1",
                QuizTitle = "JPJ Road Signs Quiz",
                Score = 3,
                TotalQuestions = 3,
                Percentage = 100,
                Passed = true,
                DateTaken = "2026-07-21"
            });
        }
    }
}
