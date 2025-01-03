using System.Text.Json.Serialization;

namespace Pawgress.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SensorType
    {
        Accelerometer,
        Gyroscope,
        Temperature,
        GPS,
        Loopsnelheid,
        Other // Voor toekomstige sensoren
    }
}
