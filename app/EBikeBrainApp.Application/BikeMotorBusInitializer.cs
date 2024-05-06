using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions.Events;
using EBikeBrainApp.Domain.Events;

namespace EBikeBrainApp.Application;

public class BikeMotorBusInitializer : IEventBusInitializer
{
    public void Initialize(IEventBus bus)
    {
        bus.AddStream(
            bus.GetStream<BikeMotorConnected>()
                .Select(x => x.BikeMotor.RotationalSpeed)
                .Switch()
                .Select(WheelRotationalSpeed.From));
    }
}
