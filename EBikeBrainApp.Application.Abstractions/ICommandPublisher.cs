namespace EBikeBrainApp.Application.Abstractions;

public interface ICommandPublisher<T>
    where T : notnull
{
    Task Publish(T command);
}
