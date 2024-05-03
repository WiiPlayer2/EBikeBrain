using System.Text.Json;
using System.Text.Json.Serialization;
using EBikeBrainApp.Application.Abstractions;
using LanguageExt;

namespace EBikeBrainApp.Implementations.Demo;

public class DemoConfigurationStore<T> : IConfigurationStore<T>
{
    private readonly string filename = $"{typeof(T).Name}.json";

    private readonly JsonSerializerOptions serializerOptions = new()
    {
        Converters =
        {
            new OptionConverterFactory(),
        },
    };

    private Option<T> config;

    public async Task<Option<T>> Load(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filename))
            return Option<T>.None;

        return await File.ReadAllTextAsync(filename, cancellationToken)
            .Map(x => JsonSerializer.Deserialize<T>(x, serializerOptions));
    }

    public Task Store(T config, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(config, serializerOptions);
        return File.WriteAllTextAsync(filename, json, cancellationToken);
    }
}

internal class OptionConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Option<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
        (JsonConverter) Activator.CreateInstance(typeof(OptionConverter<>).MakeGenericType(typeToConvert.GenericTypeArguments[0]))!;
}

internal class OptionConverter<T> : JsonConverter<Option<T>>
{
    public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        JsonSerializer.Deserialize<T[]>(ref reader, options).ToOption();

    public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize<IEnumerable<T>>(writer, value, options);
}
