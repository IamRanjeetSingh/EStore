using System.Text.Json.Serialization;
using System.Text.Json;

namespace UserService.API.Utils.JsonSerialization
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private readonly string serializationFormat;

        public DateOnlyJsonConverter() : this(null)
        {
        }

        public DateOnlyJsonConverter(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
        }

        public override DateOnly Read(ref Utf8JsonReader reader,
                                Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            if (string.IsNullOrEmpty(value))
                throw new NotSupportedException("Null or Empty value is not supported for DateOnly.");
            return DateOnly.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(serializationFormat));
        }
    }
}
