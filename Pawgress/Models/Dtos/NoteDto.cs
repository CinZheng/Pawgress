public class NoteDto
{
    public Guid? NoteId { get; set; }
    public Guid DogProfileId { get; set; }
    public Guid UserId { get; set; }
    public string? Tag { get; set; }
    public string Description { get; set; }
    public string? UserName { get; set; } // hoeft niet in model, anders is het redundant 
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }
}
