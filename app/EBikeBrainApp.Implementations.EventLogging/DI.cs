using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EBikeBrainApp.Implementations.EventLogging;

public static class DI
{
    public static void AddEventLogging(this ILoggingBuilder builder)
    {
        builder.Services.AddSingleton<ILoggerProvider, EventLoggerProvider>();
    }
}
