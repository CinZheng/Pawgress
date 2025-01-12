using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pawgress.Dtos
{
    public class TrainingPathDto
    {
        public Guid TrainingPathId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Unified list of polymorphic items
        public List<TrainingPathItemDto> LessonsQuizzes { get; set; } = new List<TrainingPathItemDto>();

        public List<UserTrainingPathDto>? Users { get; set; } = new List<UserTrainingPathDto>();
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }


    [JsonConverter(typeof(TrainingPathItemDtoConverter))]
    public abstract class TrainingPathItemDto
    {
        public Guid Id { get; set; }
        public int Order { get; set; }

        protected TrainingPathItemDto() { }
    }
}
