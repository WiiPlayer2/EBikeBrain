using EBikeBrainApp.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace EBikeBrain.Implementations.Eventing;

public class CommandPublisher<T>(ICommandHandler<T> commandHandler, ILogger<CommandPublisher<T>> logger) : ICommandPublisher<T>
    where T : notnull
{
    public async Task Publish(T command)
    {
        logger.LogTrace(">> {command}", command);
        try
        {
            await commandHandler.ExecuteAsync(command);
        }
        catch (Exception e)
        {
            logger.LogError(e, "!! {command} -> {errorType}: {errorMessage}", command, e.GetType().FullName, e.Message);
        }
    }
}
