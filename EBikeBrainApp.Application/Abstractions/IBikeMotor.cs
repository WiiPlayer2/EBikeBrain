namespace EBikeBrainApp.Application.Abstractions;

public interface IBikeMotor
{
    IObservable<RotationalSpeed> RotationalSpeed { get; }
}
