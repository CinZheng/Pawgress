using System;
using System.Collections.Generic;

namespace Pawgress.Models
{
    public class Quiz : TrainingPathItem
    {
        public string? Name { get; set; }

        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<LibraryQuiz> LibraryQuizzes { get; set; } = new List<LibraryQuiz>();
        public List<QuizQuestion>? QuizQuestions { get; set; } = new List<QuizQuestion>();


    }
}
