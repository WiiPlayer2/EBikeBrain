using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions;

namespace EBikeBrain.Implementations.Eventing;

public class EventStream<T>(EventBus eventBus) : IEventStream<T> where T : notnull
{
    private readonly IObservable<T> eventsWithBuffer = eventBus
        .OfType<T>()
        .Replay(1)
        .RefCount();

    public void Publish(T value) => eventBus.OnNext(value);

    public IDisposable Subscribe(IObserver<T> observer) => eventsWithBuffer
        .Subscribe(observer);
}
