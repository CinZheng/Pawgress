using System;

namespace Pawgress.Models
{
    public class DogSensorData
    {
        public Guid DogSensorDataId { get; set; } 
        public string Name { get; set; } // naam van de sensor
        public string Description { get; set; } 
        public string SensorType { get; set; } // bv "Accelerometer", "Gyroscope"
        public string Unit { get; set; } // bv "m/s²" of "°/s"
        public DateTime RecordedDate { get; set; } 
        public double AverageValue { get; set; } // gemiddelde gemeten waarde
    }
}
