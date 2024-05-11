using System.Reactive.Disposables;
using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace EBikeBrainApp.Application;

public class EventTraceLogging(ILogger<EventTraceLogging> logger) : IEventBusInitializer, IDisposable
{
    private CompositeDisposable? subscription;

    public void Dispose()
    {
        subscription?.Dispose();
    }

    public void Initialize(IEventBus bus)
    {
        subscription = new CompositeDisposable(
            bus.GetStream<object>()
                .Where(x => x is not LogEntry and not EventStreamError)
                .Subscribe(x => logger.LogTrace("<<{event}>>", x)),
            bus.GetStream<object>()
                .OfType<EventStreamError>()
                .Subscribe(x => logger.LogError(x.Exception, "<<{stream}>> {errorType}: {errorMessage}", x.StreamType.Name, x.Exception.GetType().FullName, x.Exception.Message)));
    }
}
