using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Application.Abstractions.Events;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace EBikeBrainApp.Implementations.EventLogging;

public class EventLogger(IEventStream<LogEntry> logStream, string categoryName) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => throw new NotImplementedException();

    public bool IsEnabled(LogLevel logLevel) => throw new NotImplementedException();

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        // logStream.Publish(LogEntry.From($"[{logLevel}] {categoryName}: {formatter(state, exception)}"));
        logStream.Publish(new LogEntry(
            logLevel,
            categoryName,
            Prelude.Optional(exception),
            formatter(state, exception)));
    }
}
