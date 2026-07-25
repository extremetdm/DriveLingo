using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DriveLingo.Database.Seeders
{
    public static class QuizSeeder
    {
        public static void Run(AppDbContext db)
        {
            var quiz1 = new Quiz
            {
                Id = 1,
                ModuleId = 1,
                Title = "JPJ Road Signs Quiz",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Text = "What does the road sign shown below indicate?",
                        Image = "uploads/no_entry.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "No Parking Zone", IsCorrect = false },
                            new QuestionChoice { Text = "No Entry (Dilarang Masuk)", IsCorrect = true },
                            new QuestionChoice { Text = "Stop Command", IsCorrect = false },
                            new QuestionChoice { Text = "Speed Limit Ahead", IsCorrect = false }
                        }
                    },
                    new Question
                    {
                        Text = "What does this yellow diamond road sign warn drivers about?",
                        Image = "uploads/warning_curve.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "Slippery Road", IsCorrect = false },
                            new QuestionChoice { Text = "Sharp Right Curve Ahead", IsCorrect = true },
                            new QuestionChoice { Text = "Narrow Bridge", IsCorrect = false },
                            new QuestionChoice { Text = "Roundabout Ahead", IsCorrect = false }
                        }
                    },
                    new Question
                    {
                        Text = "What type of highway route is indicated by the green background sign below?",
                        Image = "uploads/expressway_sign.svg",
                        Choices = new List<QuestionChoice>
                        {
                            new QuestionChoice { Text = "State Highway", IsCorrect = false },
                            new QuestionChoice { Text = "Malaysian Expressway (Lebuhraya)", IsCorrect = true },
                            new QuestionChoice { Text = "Federal Route", IsCorrect = false },
                            new QuestionChoice { Text = "Town Municipal Road", IsCorrect = false }
                        }
                    }
                }
            };

            db.Quizzes.AddOrUpdate(quiz1);
            db.SaveChanges();
        }
    }
}