using System.Text.Json;
using System.Text.Json.Serialization;
using Pawgress.Dtos;

public class TrainingPathItemDtoConverter : JsonConverter<TrainingPathItemDto>
{
    public override TrainingPathItemDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (var document = JsonDocument.ParseValue(ref reader))
        {
            var root = document.RootElement;
            if (root.TryGetProperty("type", out var typeProperty))
            {
                var type = typeProperty.GetString();
                return type switch
                {
                    "lesson" => JsonSerializer.Deserialize<LessonDto>(root.GetRawText(), options),
                    "quiz" => JsonSerializer.Deserialize<QuizDto>(root.GetRawText(), options),
                    _ => throw new JsonException($"Unknown type: {type}")
                };
            }

            throw new JsonException("Missing type property");
        }
    }

    public override void Write(Utf8JsonWriter writer, TrainingPathItemDto value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
