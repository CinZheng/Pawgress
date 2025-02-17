using System;

namespace Pawgress.Models
{
    public class User_DogProfile
    {
        public Guid UserId { get; set; }
        public Guid DogProfileId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFavorite { get; set; }
        public User User { get; set; }
        public DogProfile DogProfile { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
