using Microsoft.Extensions.Logging;

namespace EBikeBrainApp.Application.Abstractions.Events;

public readonly record struct LogEntry(
    LogLevel Level,
    string CategoryName,
    Option<Exception> Exception,
    string Message);
