using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;

namespace EBikeBrain.Implementations.Eventing;

public class CommandPublisher<T>(ICommandHandler<T> commandHandler, IEventStream<LogEntry> logs) : ICommandPublisher<T>
    where T : notnull
{
    public Task Publish(T command)
    {
        logs.Publish(LogEntry.From($">> {command}"));
        return commandHandler.ExecuteAsync(command);
    }
}
