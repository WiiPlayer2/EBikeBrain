using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions;

namespace EBikeBrain.Implementations.Eventing;

public interface IEventProjector;

public class EventProjector<T1, T2, TOut, TProjection>(
    IEventStream<T1> inputStream1,
    IEventStream<T2> inputStream2,
    IEventStream<TOut> outputStream
) : IEventProjector, IDisposable
    where T1 : notnull
    where T2 : notnull
    where TOut : notnull
    where TProjection : IEventProjection<T1, T2, TOut>
{
    private readonly IDisposable subscription = inputStream1
        .CombineLatest(inputStream2)
        .Select(t => TProjection.Project(t.First, t.Second))
        .Subscribe(outputStream.Publish);

    public void Dispose()
    {
        subscription.Dispose();
    }
}
