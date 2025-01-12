using System;

namespace Pawgress.Models
{
    public class UserProgress
    {
        public Guid UserProgressId { get; set; }
        public Guid UserId { get; set; }
        public Guid TrainingPathId { get; set; }
        public Guid ItemId { get; set; }  // Can be either LessonId or QuizId
        public string ItemType { get; set; }  // "Lesson" or "Quiz"
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int Score { get; set; }  // For quizzes
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual TrainingPath TrainingPath { get; set; }
    }
} 