using Pawgress.Models;

public class LibraryQuiz
{
    public Guid LibraryId { get; set; }
    public Library Library { get; set; }

    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; }
}
