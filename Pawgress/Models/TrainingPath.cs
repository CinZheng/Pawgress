using System;
using System.Collections.Generic;

namespace Pawgress.Models
{
    public class TrainingPath
    {
        public Guid TrainingPathId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
        public List<Quiz> Quizzes { get; set; } = new List<Quiz>();

        public List<User_TrainingPath> Users { get; set; } = new List<User_TrainingPath>();
    }
}
