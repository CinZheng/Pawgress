using Pawgress.Models;

public class DogProfileDto
{
    public Guid DogProfileId { get; set; }
    public string Name { get; set; }
    public string Breed { get; set; }
    public string Image { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<string>? Notes { get; set; }
    public List<DogSensorData>? DogSensorDatas { get; set; } 
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }
}
