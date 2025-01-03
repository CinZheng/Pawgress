using System;

namespace Pawgress.Models
{
    public class DogSensorData
    {
        public Guid DogSensorDataId { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; }
        public SensorType SensorType { get; set; } // enum
        public string Unit { get; set; } 
        public DateTime RecordedDate { get; set; } 
        public double AverageValue { get; set; }
        public Guid DogProfileId { get; set; }
        public DogProfile DogProfile { get; set; }

    }
}
