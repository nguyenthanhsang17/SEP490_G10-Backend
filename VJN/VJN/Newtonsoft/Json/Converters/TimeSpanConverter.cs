
namespace Newtonsoft.Json.Converters
{
    public class TimeSpanConverter : JsonConverter<TimeSpan?>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan? value, JsonSerializer serializer)
        {
            writer.WriteValue(value.HasValue ? value.Value.ToString(@"hh\:mm\:ss") : null);
        }

        public override TimeSpan? ReadJson(JsonReader reader, Type objectType, TimeSpan? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();
            return TimeSpan.TryParse(value, out var result) ? result : (TimeSpan?)null;
        }
    }
}