using System.Reactive.Linq;
using EBikeBrainApp.Domain.Events;

namespace EBikeBrainApp.Application;

public class BikeSpeedCalculation(
    ConfigurationService configurationService)
    : IEventBusInitializer
{
    public void Initialize(IEventBus bus)
    {
        bus.AddStream(
            bus.GetStream<WheelRotationalSpeed>()
                .CombineLatest(configurationService.Bike, (speed, configuration) => (speed, configuration))
                .Select(t => BikeSpeed.From(t.speed.Value.ToLinearSpeed(t.configuration.WheelDiameter.Value))));
    }
}
