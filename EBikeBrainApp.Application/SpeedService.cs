using System.Reactive.Linq;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class SpeedService<RT>(
    BikeMotorService<RT> bikeMotorService,
    ConfigurationService configurationService)
    where RT : struct, HasCancel<RT>
{
    public IObservable<Fin<Speed>> Speed { get; } = bikeMotorService.RotationalSpeed
        .CombineLatest(configurationService.Bike.Select(x => x.WheelDiameter))
        .Select(t => t.First.Map(x => x.ToLinearSpeed(t.Second.Value)));
}
