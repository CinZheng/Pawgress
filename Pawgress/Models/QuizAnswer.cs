using System;

namespace Pawgress.Models
{
    public class QuizAnswer
    {
        public Guid QuizAnswerId { get; set; }
        public Guid QuizId { get; set; }
        public string Question { get; set; }
        public string? UserAnswer { get; set; }
        public string? CorrectAnswer { get; set; }
    }
}
