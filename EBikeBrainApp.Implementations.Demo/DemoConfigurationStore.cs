using EBikeBrainApp.Implementations.JsonConfigurationStore;
using LanguageExt;

namespace EBikeBrainApp.Implementations.Demo;

public class DemoConfigurationStore<T> : JsonConfigurationStore<T>
{
    private readonly string filename = $"{typeof(T).Name}.json";

    protected override async Task<Option<string>> LoadJson(CancellationToken cancellationToken = default) =>
        File.Exists(filename)
            ? await File.ReadAllTextAsync(filename, cancellationToken)
            : Option<string>.None;

    protected override Task StoreJson(string json, CancellationToken cancellationToken = default) =>
        File.WriteAllTextAsync(filename, json, cancellationToken);
}
