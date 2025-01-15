using System;

namespace Pawgress.Models
{
    public class QuizAnswer
    {
        public Guid QuizAnswerId { get; set; }
        public Guid UserId { get; set; }
        public Guid QuizQuestionId { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        // Navigation properties
        public User User { get; set; }
        public QuizQuestion QuizQuestion { get; set; }
    }
} 