using System;
using System.Collections.Generic;

namespace Pawgress.Dtos
{
    public class TrainingPathDto
    {
        public Guid TrainingPathId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<LessonDto>? Lessons { get; set; } = new List<LessonDto>();
        public List<QuizDto>? Quizzes { get; set; } = new List<QuizDto>();
        public List<UserTrainingPathDto>? Users { get; set; } = new List<UserTrainingPathDto>();
    }
}
