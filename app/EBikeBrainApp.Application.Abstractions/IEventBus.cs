namespace EBikeBrainApp.Application.Abstractions;

public interface IEventBus
{
    public void AddStream<T>(IObservable<T> stream)
        where T : notnull;

    public IObservable<T> GetStream<T>()
        where T : notnull;
}
