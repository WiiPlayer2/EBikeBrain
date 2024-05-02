using EBikeBrainApp.Application.Abstractions;
using LanguageExt;

namespace EBikeBrainApp.Implementations.Demo;

public class DemoConfigurationStore : IConfigurationStore
{
    private Option<BikeConfiguration> bikeConfiguration;

    private Option<BikeConnectionConfiguration> connectionConfiguration;

    public Task<Option<BikeConfiguration>> LoadBikeConfig(CancellationToken cancellationToken = default) =>
        Task.FromResult(bikeConfiguration);

    public Task<Option<BikeConnectionConfiguration>> LoadConnectionConfig(CancellationToken cancellationToken = default) =>
        Task.FromResult(connectionConfiguration);

    public Task StoreBikeConfig(BikeConfiguration config, CancellationToken cancellationToken = default)
    {
        bikeConfiguration = config;
        return Task.CompletedTask;
    }

    public Task StoreConnectionConfig(BikeConnectionConfiguration config, CancellationToken cancellationToken = default)
    {
        connectionConfiguration = config;
        return Task.CompletedTask;
    }
}
