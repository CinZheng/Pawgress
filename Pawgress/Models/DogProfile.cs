using System;
using System.Collections.Generic;

namespace Pawgress.Models
{
    public class DogProfile
    {
        public Guid DogProfileId { get; set; }
        public string Name { get; set; }
        public string? Breed { get; set; }
        public string? Image { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        // relatie met Notes 
        public List<Note>? Notes { get; set; } = new List<Note>();
        // relatie met Users
        public List<User_DogProfile>? UserDogProfiles { get; set; } = new List<User_DogProfile>();
        // relatie met DogSensorData
        public List<DogSensorData>? DogSensorDatas { get; set; } = new List<DogSensorData>();

    }
}
