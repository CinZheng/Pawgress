using System;

namespace Pawgress.Dtos
{
    public class UserTrainingPathDto
    {
        public Guid UserId { get; set; }
        public Guid TrainingPathId { get; set; }
        public string? Progress { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
