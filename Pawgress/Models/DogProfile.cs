using System;
using System.Collections.Generic;

namespace Pawgress.Models
{
    public class DogProfile
    {
        public Guid DogProfileId { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Note> Notes { get; set; } = new List<Note>();
        public List<User_DogProfile> UserProfiles { get; set; }

        public List<User_DogProfile> UserDogProfiles { get; set; } = new List<User_DogProfile>();
    }
}
