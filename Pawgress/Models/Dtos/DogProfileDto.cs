public class DogProfileDto
{
    public Guid DogProfileId { get; set; }
    public string Name { get; set; }
    public string Breed { get; set; }
    public string Image { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<string>? Notes { get; set; }
}
