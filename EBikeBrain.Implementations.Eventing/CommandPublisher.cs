using EBikeBrainApp.Application.Abstractions;

namespace EBikeBrain.Implementations.Eventing;

public class CommandPublisher<T>(ICommandHandler<T> commandHandler) : ICommandPublisher<T>
    where T : notnull
{
    public Task Publish(T command) => commandHandler.ExecuteAsync(command);
}
