using System.Reactive;
using System.Reactive.Subjects;

namespace EBikeBrainApp.Utils;

public class Busy : IDisposable
{
    private readonly BehaviorSubject<bool> isBusySubject = new(false);

    private readonly SemaphoreSlim mutex = new(1, 1);

    public IObservable<bool> IsBusy => isBusySubject;

    public void Dispose()
    {
        mutex.Dispose();
        isBusySubject.Dispose();
    }

    public Task Run(Func<Task> fn, CancellationToken cancellationToken = default) =>
        Run(async () =>
        {
            await fn();
            return Unit.Default;
        }, cancellationToken);

    public async Task<T> Run<T>(Func<Task<T>> fn, CancellationToken cancellationToken = default)
    {
        try
        {
            await mutex.WaitAsync(cancellationToken);
            isBusySubject.OnNext(true);
            return await fn();
        }
        finally
        {
            isBusySubject.OnNext(false);
            mutex.Release();
        }
    }
}
