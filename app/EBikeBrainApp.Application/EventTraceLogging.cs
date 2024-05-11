using System.Reactive.Linq;
using Microsoft.Extensions.Logging;

namespace EBikeBrainApp.Application;

public class EventTraceLogging(ILogger<EventTraceLogging> logger) : IEventBusInitializer, IDisposable
{
    private IDisposable? subscription;

    public void Dispose()
    {
        subscription?.Dispose();
    }

    public void Initialize(IEventBus bus)
    {
        subscription = bus.GetStream<object>()
            .Where(x => x is not LogEntry)
            .Subscribe(x => logger.LogTrace("<<{event}>>", x));
    }
}
