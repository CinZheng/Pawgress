using System;

namespace Pawgress.Models.Dtos
{
    public class CreateDogSensorDataDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SensorType SensorType { get; set; }
        public string Unit { get; set; }
        public double AverageValue { get; set; }
        public Guid DogProfileId { get; set; }
    }
} 