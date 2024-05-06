using System.Reactive.Linq;
using EBikeBrainApp.Domain.Events;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class SpeedService<RT> where RT : struct, HasCancel<RT>
{
    public SpeedService(BikeMotorService<RT> bikeMotorService,
        ConfigurationService configurationService,
        IEventBus eventBus)
    {
        Speed = bikeMotorService.RotationalSpeed
            .CombineLatest(configurationService.Bike.Select(x => x.WheelDiameter))
            .Select(t => t.First.Map(x => x.ToLinearSpeed(t.Second.Value)));

        eventBus.AddStream(
            bikeMotorService.RotationalSpeed
                .Where(x => x.IsSucc)
                .Select(x => WheelRotationalSpeed.From((RotationalSpeed) x.Case)));

        eventBus.AddStream(
            // eventBus.GetStream<BikeConfiguration>() // TODO
            configurationService.Bike
                .CombineLatest(eventBus.GetStream<WheelRotationalSpeed>())
                .Select(t => BikeSpeed.From(t.Second.Value.ToLinearSpeed(t.First.WheelDiameter.Value))));
    }

    public IObservable<Fin<Speed>> Speed { get; }
}
