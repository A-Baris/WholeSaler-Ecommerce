using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class PolymorphicJsonConverter<T> : JsonConverter<T>
{
    public override bool CanConvert(Type typeToConvert) => typeof(T).IsAssignableFrom(typeToConvert);

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var typeDiscriminator = doc.RootElement.GetProperty("typeDiscriminator").GetString();
            var type = Type.GetType(typeDiscriminator);
            return (T)JsonSerializer.Deserialize(doc.RootElement.GetRawText(), type, options);
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var type = value.GetType();
        JsonSerializer.Serialize(writer, value, type, options);
    }
}