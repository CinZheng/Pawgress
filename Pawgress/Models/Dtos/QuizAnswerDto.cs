using System;

namespace Pawgress.Dtos
{
    public class QuizAnswerDto
    {
        public Guid QuizAnswerId { get; set; }
        public Guid UserId { get; set; }
        public Guid QuizQuestionId { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
} 