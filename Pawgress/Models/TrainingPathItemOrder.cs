using Pawgress.Models;

public class TrainingPathItemOrder
{
    public Guid Id { get; set; }
    public Guid TrainingPathId { get; set; }
    public TrainingPath TrainingPath { get; set; }

    public Guid TrainingPathItemId { get; set; } // id van de quiz/les
    public TrainingPathItem TrainingPathItem { get; set; }

    public int Order { get; set; }
}
