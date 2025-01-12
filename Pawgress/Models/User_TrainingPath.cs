using System;

namespace Pawgress.Models
{
    public class User_TrainingPath
    {
        public Guid UserId { get; set; }
        public Guid TrainingPathId { get; set; }
        public string? Progress { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual TrainingPath TrainingPath { get; set; }
    }
}
