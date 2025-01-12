using Pawgress.Models;

public class LibraryLesson
{
    public Guid LibraryId { get; set; }
    public Library Library { get; set; }

    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; }
}
