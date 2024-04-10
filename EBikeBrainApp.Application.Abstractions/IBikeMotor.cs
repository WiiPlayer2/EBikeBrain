namespace EBikeBrainApp.Application.Abstractions;

public interface IBikeMotor
{
    IObservable<PasLevel> PasLevel { get; }

    IObservable<RotationalSpeed> RotationalSpeed { get; }

    ValueTask SetPasLevel(PasLevel level, CancellationToken cancellationToken = default);
}