using System;
using System.Collections.Generic;

namespace DriveLingo.Models
{
    public class Question
    {
        public string Id { get; set; }
        public string QuizId { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; }
        public string ImageUrl { get; set; }

        public Question()
        {
            Options = new List<string>();
        }
    }

    public class Quiz
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; } // "Road Signs", "Rules & Safety", etc.
        public int RewardPoints { get; set; }
        public List<Question> Questions { get; set; }

        public Quiz()
        {
            Questions = new List<Question>();
        }
    }

    public class QuizAttempt
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int Percentage { get; set; }
        public bool Passed { get; set; }
        public string DateTaken { get; set; }
    }
}
