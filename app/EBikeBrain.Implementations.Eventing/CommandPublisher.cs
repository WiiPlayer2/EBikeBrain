using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;

namespace EBikeBrain.Implementations.Eventing;

public class CommandPublisher<T>(ICommandHandler<T> commandHandler, IEventStream<LogEntry> logs) : ICommandPublisher<T>
    where T : notnull
{
    public async Task Publish(T command)
    {
        logs.Publish(LogEntry.From($">> {command}"));
        try
        {
            await commandHandler.ExecuteAsync(command);
        }
        catch (Exception e)
        {
            logs.Publish(LogEntry.From($"!! {command} -> {e}"));
        }
    }
}
