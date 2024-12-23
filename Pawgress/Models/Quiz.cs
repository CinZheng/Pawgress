using System;
using System.Collections.Generic;

namespace Pawgress.Models
{
    public class Quiz
    {
        public Guid QuizId { get; set; }
        public string QuizName { get; set; }
        public string QuizDescription { get; set; }
        public Guid? TrainingPathId { get; set; }
        public TrainingPath? TrainingPath { get; set; }
        public List<QuizQuestion>? QuizQuestions { get; set; } = new List<QuizQuestion>();

    }
}
