using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Pawgress.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string? UserPicture { get; set; }
        public string Email { get; set; }
        public string? ProgressData { get; set; }
        public string Role { get; set; }
        public string PasswordHash { get; set; }
        public List<Note>? Notes { get; set; } = new List<Note>();
         public List<User_DogProfile>? DogProfiles { get; set; } = new List<User_DogProfile>();
         public List<User_TrainingPath>? TrainingPaths { get; set; } = new List<User_TrainingPath>();
    }
}
