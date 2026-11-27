using System.Text.Json;
using System.Text.Json.Serialization;

namespace NextHire_API.Infrastructure.Json
{
    public class UtcDateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DateTime.Parse(reader.GetString()!, null, System.Globalization.DateTimeStyles.RoundtripKind);

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            DateTime utc = value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
            };

            writer.WriteStringValue(utc.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
        }
    }
}
