using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using EBikeBrainApp.Application.Abstractions;

namespace EBikeBrain.Implementations.Eventing;

public class EventBus : IEventBus, ISubject<object>, IDisposable
{
    private readonly Subject<object> baseSubject = new();

    private readonly CompositeDisposable streamSubscriptions = new();

    public void Dispose()
    {
        streamSubscriptions.Dispose();
        baseSubject.Dispose();
    }

    public void AddStream<T>(IObservable<T> stream)
        where T : notnull =>
        streamSubscriptions.Add(stream.Subscribe(x => OnNext(x)));

    public IObservable<T> GetStream<T>()
        where T : notnull =>
        this.OfType<T>();

    public IDisposable Subscribe(IObserver<object> observer) => baseSubject.Subscribe(observer);

    public void OnCompleted()
    {
        baseSubject.OnCompleted();
    }

    public void OnError(Exception error)
    {
        baseSubject.OnError(error);
    }

    public void OnNext(object value)
    {
        baseSubject.OnNext(value);
    }
}
