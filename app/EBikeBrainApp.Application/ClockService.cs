using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions.Events;
using LanguageExt.UnitsOfMeasure;

namespace EBikeBrainApp.Application;

public class ClockService(IClock clock) : IEventBusInitializer
{
    public void Initialize(IEventBus bus)
    {
        bus.AddStream(
            Observable.Timer(0.Seconds(), 10.Seconds())
                .Select(_ => ClockTime.From(clock.Now)));
    }
}
