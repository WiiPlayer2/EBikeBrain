using System.Reactive.Subjects;

namespace EBikeBrain.Implementations.Eventing;

public class EventBus : ISubject<object>
{
    private readonly Subject<object> baseSubject = new();

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
