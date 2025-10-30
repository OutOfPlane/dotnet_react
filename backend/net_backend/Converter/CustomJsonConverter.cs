using System.Collections;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class GZipHelper
{
    public static string CompressToBase64(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(output, CompressionLevel.Optimal))
        {
            gzip.Write(bytes, 0, bytes.Length);
        }

        return Convert.ToBase64String(output.ToArray());
    }
}

public class CustomJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsClass || typeToConvert == typeof(string))
            return false;
        if (typeToConvert.IsArray)
            return false;
        if (typeof(IEnumerable).IsAssignableFrom(typeToConvert))
            return false;
        return true;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(CustomJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public class CustomJsonConverter<T> : JsonConverter<T> where T : class
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (T?)JsonSerializer.Deserialize(ref reader, typeToConvert);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.GetCustomAttribute<SensitiveAttribute>() != null)
                continue;

            var propValue = property.GetValue(value);
            var propName = options.PropertyNamingPolicy?.ConvertName(property.Name) ?? property.Name;
            writer.WritePropertyName(propName);

            if (property.GetCustomAttribute<CompressedAttribute>() != null)
            {
                var json = JsonSerializer.Serialize(propValue, propValue?.GetType() ?? typeof(object), options);
                var compressed = GZipHelper.CompressToBase64(json);
                writer.WriteStringValue(compressed);
                continue;
            }

            
            JsonSerializer.Serialize(writer, propValue, propValue?.GetType() ?? typeof(object), options);
        }

        writer.WriteEndObject();
    }

}