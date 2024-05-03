using System.Text.Json;
using EBikeBrainApp.Application.Abstractions;
using LanguageExt;

namespace EBikeBrainApp.Implementations.JsonConfigurationStore;

public abstract class JsonConfigurationStore<T> : IConfigurationStore<T>
{
    // TODO should be static in a non-generic type
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        Converters =
        {
            new OptionConverterFactory(),
        },
    };

    public async Task<Option<T>> Load(CancellationToken cancellationToken = default) =>
        from json in await LoadJson(cancellationToken)
        let config = JsonSerializer.Deserialize<T>(json, serializerOptions)
        select config;

    public Task Store(T config, CancellationToken cancellationToken = default) =>
        StoreJson(JsonSerializer.Serialize(config, serializerOptions), cancellationToken);

    protected abstract Task<Option<string>> LoadJson(CancellationToken cancellationToken = default);

    protected abstract Task StoreJson(string json, CancellationToken cancellationToken = default);
}
