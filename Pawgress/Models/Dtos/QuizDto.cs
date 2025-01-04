using System;
using System.Collections.Generic;

namespace Pawgress.Dtos
{
    public class QuizDto
    {
        public Guid? QuizId { get; set; } 
        public string QuizName { get; set; }
        public string? QuizDescription { get; set; }
        public Guid? TrainingPathId { get; set; }
        public List<QuizQuestionDto>? QuizQuestions { get; set; } = new List<QuizQuestionDto>();
         public int SortOrder { get; set; } // voor trainingpath
    }

    public class QuizQuestionDto
    {
        public Guid? QuizQuestionId { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public string? MediaUrl { get; set; }
    }
}
