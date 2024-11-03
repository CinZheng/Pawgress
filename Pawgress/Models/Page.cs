using System;

namespace Pawgress.Models
{
    public class Page
    {
        public Guid PageId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Video { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Tag { get; set; }

        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
