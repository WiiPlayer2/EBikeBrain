using System.Reactive.Linq;

namespace EBikeBrainApp.Application;

public class BikeService(
    BikeMotorService bikeMotorService,
    Length wheelDiameter)
{
    public IObservable<Fin<Speed>> Speed { get; } = bikeMotorService.RotationalSpeed
        .Select(x => x.Map(x => x.ToLinearSpeed(wheelDiameter)));
}
