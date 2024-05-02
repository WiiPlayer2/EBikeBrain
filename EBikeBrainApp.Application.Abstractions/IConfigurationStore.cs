namespace EBikeBrainApp.Application.Abstractions;

public interface IConfigurationStore
{
    Task<Option<BikeConfiguration>> LoadBikeConfig(CancellationToken cancellationToken = default);

    Task<Option<BikeConnectionConfiguration>> LoadConnectionConfig(CancellationToken cancellationToken = default);

    Task StoreBikeConfig(BikeConfiguration config, CancellationToken cancellationToken = default);

    Task StoreConnectionConfig(BikeConnectionConfiguration config, CancellationToken cancellationToken = default);
}
