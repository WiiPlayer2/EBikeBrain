using System.Reactive.Linq;
using EBikeBrainApp.Domain.Events;

namespace EBikeBrainApp.Application.Eventing;

public class BikeMotorCalculations(
    ConfigurationService configurationService)
    : IEventBusInitializer
{
    public void Initialize(IEventBus bus)
    {
        bus.AddStream(
            bus.GetStream<WheelRotationalSpeed>()
                .CombineLatest(configurationService.Bike, (speed, configuration) => (speed, configuration))
                .Select(t => BikeSpeed.From(t.speed.Value.ToLinearSpeed(t.configuration.WheelDiameter.Value))));

        bus.AddStream(
            bus.GetStream<BikeMotorCurrent>()
                .CombineLatest(configurationService.Bike, (current, configuration) => (current, configuration))
                .Select(t => BikeMotorPower.From(t.current.Value * t.configuration.MotorVoltage.Value)));
    }
}
