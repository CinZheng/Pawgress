using System;

namespace Pawgress.Models
{
    public abstract class TrainingPathItem
    {
        public Guid Id { get; set; } // id is voor quiz en lesson
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid? TrainingPathId { get; set; }
        public List<TrainingPathItemOrder> TrainingPaths { get; set; } = new List<TrainingPathItemOrder>();
    }
}
