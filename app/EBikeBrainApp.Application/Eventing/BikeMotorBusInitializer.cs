using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions.Events;
using EBikeBrainApp.Domain.Events;

namespace EBikeBrainApp.Application.Eventing;

public class BikeMotorBusInitializer : IEventBusInitializer
{
    public void Initialize(IEventBus bus)
    {
        bus.AddStream(
            bus.GetStream<BikeMotorConnected>()
                .Select(x => x.BikeMotor.RotationalSpeed)
                .Switch()
                .Select(WheelRotationalSpeed.From));

        bus.AddStream(
            bus.GetStream<BikeMotorConnected>()
                .Select(x => x.BikeMotor.Current)
                .Switch()
                .Select(BikeMotorCurrent.From));

        bus.AddStream(
            bus.GetStream<BikeMotorConnected>()
                .Select(x => x.BikeMotor.Battery)
                .Switch()
                .Select(BikeMotorBatteryPercentage.From));

        bus.AddStream(
            bus.GetStream<BikeMotorConnected>()
                .Select(x => x.BikeMotor.PasLevel)
                .Switch()
                .Select(BikeMotorPasLevel.From));
    }
}
