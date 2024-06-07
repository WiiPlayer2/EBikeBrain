using System.Reactive.Linq;
using EBikeBrainApp.Domain.Events;

namespace EBikeBrainApp.Application.Eventing;

public class BikeDistances : IEventBusInitializer
{
    public void Initialize(IEventBus bus)
    {
        bus.AddStream(
            bus.GetStream<BikeSpeed>()
                .StartWith(BikeSpeed.From(Speed.Zero))
                .TimeInterval()
                .Scan(Length.Zero, (acc, cur) => acc + cur.Interval * cur.Value.Value)
                .Select(BikeDistanceCycled.From)
                .StartWith(BikeDistanceCycled.From(Length.Zero)));
    }
}
