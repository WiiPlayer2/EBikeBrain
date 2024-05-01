namespace EBikeBrainApp.Application.Abstractions;

public interface IEventStream<T> : IObservable<T>
    where T : notnull
{
    void Publish(T value);
}
