using EBikeBrainApp.Implementations.JsonConfigurationStore;
using LanguageExt;
using Microsoft.Maui.Storage;

namespace EBikeBrainApp.Implementations.Android;

public class AndroidConfigurationStore<T> : JsonConfigurationStore<T>
{
    private static readonly string key = typeof(T).FullName ?? typeof(T).Name;

    protected override Task<Option<string>> LoadJson(CancellationToken cancellationToken = default) =>
        Task.FromResult(Prelude.Optional(Preferences.Get(key, default(string))));

    protected override Task StoreJson(string json, CancellationToken cancellationToken = default)
    {
        Preferences.Set(key, json);
        return Task.CompletedTask;
    }
}
