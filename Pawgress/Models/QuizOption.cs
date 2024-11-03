using System;

namespace Pawgress.Models
{
    public class QuizOption
    {
        public Guid QuizOptionId { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
