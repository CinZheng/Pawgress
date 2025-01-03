using System;

namespace Pawgress.Models
{
    public class Note
    {
        public Guid NoteId { get; set; }
        public Guid DogProfileId { get; set; }
        public Guid UserId { get; set; }
        public string? Tag { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public DogProfile DogProfile { get; set; }
        public User User { get; set; }
    }
}

