using System;

namespace Pawgress.Dtos
{
    public class LessonDto
    {
        public Guid LessonId { get; set; }
        public string Name { get; set; }
        public string? Text { get; set; }
        public string? Video { get; set; }
        public string? Image { get; set; }
        public string? MediaUrl { get; set; }
        public string? Tag { get; set; }
        public string? MarkdownContent { get; set; }
        public Guid? TrainingPathId { get; set; }
        public int SortOrder { get; set; } // voor trainingpath
    }
}
