namespace EBikeBrainApp.Application.Abstractions;

public interface ICommandHandler<T>
{
    Task ExecuteAsync(T command);
}
