namespace EBikeBrainApp.Application.Abstractions;

public interface IConfigurationStore<T>
{
    Task<Option<T>> Load(CancellationToken cancellationToken = default);

    Task Store(T config, CancellationToken cancellationToken = default);
}
