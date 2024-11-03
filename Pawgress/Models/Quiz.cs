using System;
using System.Collections.Generic;

namespace Pawgress.Models
{
    public class Quiz
    {
        public Guid QuizId { get; set; }
        public Guid TrainingPathId { get; set; }
        public string QuizName { get; set; }
        public List<QuizPage> Questions { get; set; } = new List<QuizPage>();
        public int MaxScore { get; set; }
        public int AchievedScore { get; set; }

        public TrainingPath TrainingPath { get; set; }
    }
}
