using System.Reactive.Linq;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class SpeedService<RT>(
    BikeMotorService<RT> bikeMotorService,
    WheelDiameter wheelDiameter)
    where RT : struct, HasCancel<RT>
{
    public IObservable<Fin<Speed>> Speed { get; } = bikeMotorService.RotationalSpeed
        .Select(x => x.Map(x => x.ToLinearSpeed(wheelDiameter.Value)));
}
