using System;
using System.Collections.Generic;

namespace Pawgress.Models
{
    public class QuizPage
    {
        public Guid QuizPageId { get; set; }
        public Guid QuizId { get; set; } 
        public List<QuizOption> Options { get; set; } = new List<QuizOption>();
    }
}
