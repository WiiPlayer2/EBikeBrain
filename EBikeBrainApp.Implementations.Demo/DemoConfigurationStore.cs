using EBikeBrainApp.Application.Abstractions;
using LanguageExt;

namespace EBikeBrainApp.Implementations.Demo;

public class DemoConfigurationStore<T> : IConfigurationStore<T>
{
    private Option<T> config;

    public Task<Option<T>> Load(CancellationToken cancellationToken = default) =>
        Task.FromResult(config);

    public Task Store(T config, CancellationToken cancellationToken = default)
    {
        this.config = config;
        return Task.CompletedTask;
    }
}
