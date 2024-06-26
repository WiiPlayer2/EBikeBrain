using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Application.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace EBikeBrainApp.Implementations.EventLogging;

public class EventLoggerProvider(IEventStream<LogEntry> logStream) : ILoggerProvider
{
    public void Dispose() { }

    public ILogger CreateLogger(string categoryName) => new EventLogger(logStream, categoryName);
}
