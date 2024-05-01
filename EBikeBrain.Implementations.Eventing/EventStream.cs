using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions;

namespace EBikeBrain.Implementations.Eventing;

public class EventStream<T>(EventBus eventBus) : IEventStream<T> where T : notnull
{
    public void Publish(T value) => eventBus.OnNext(value);

    public IDisposable Subscribe(IObserver<T> observer) => eventBus.OfType<T>().Subscribe(observer);
}
