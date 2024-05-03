using System.Text.Json;
using System.Text.Json.Serialization;
using LanguageExt;

namespace EBikeBrainApp.Implementations.JsonConfigurationStore;

internal class OptionConverter<T> : JsonConverter<Option<T>>
{
    public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        JsonSerializer.Deserialize<T[]>(ref reader, options).ToOption();

    public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize<IEnumerable<T>>(writer, value, options);
}
