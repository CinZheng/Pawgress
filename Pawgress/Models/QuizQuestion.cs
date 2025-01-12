using System;

namespace Pawgress.Models
{
    public class QuizQuestion
    {
        public Guid QuizQuestionId { get; set; }
        public string? QuestionText { get; set; }
        public string? CorrectAnswer { get; set; }
        public string? MediaUrl { get; set; }
        public Guid QuizId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
