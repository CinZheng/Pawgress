using System;
using System.Collections.Generic;

namespace Pawgress.Models
{
    public class TrainingPath
    {
        public Guid TrainingPathId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        
        // Single ordered list for Lessons and Quizzes
        public List<TrainingPathItemOrder> TrainingPathItems { get; set; } = new List<TrainingPathItemOrder>();

        public List<User_TrainingPath>? Users { get; set; } = new List<User_TrainingPath>();
    }
}

