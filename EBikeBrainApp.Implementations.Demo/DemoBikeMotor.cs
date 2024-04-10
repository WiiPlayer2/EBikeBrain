using System.Reactive.Linq;
using System.Reactive.Subjects;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;
using LanguageExt.UnitsOfMeasure;
using UnitsNet;

namespace EBikeBrainApp.Implementations.Demo;

public class DemoBikeMotor : IBikeMotor, IDisposable
{
    private readonly BehaviorSubject<PasLevel> pasLevelSubject = new(Domain.PasLevel.Level1);

    public IObservable<PasLevel> PasLevel => pasLevelSubject;

    public IObservable<RotationalSpeed> RotationalSpeed { get; } = Observable.Create<RotationalSpeed>(async (observer, token) =>
    {
        while (!token.IsCancellationRequested)
        {
            observer.OnNext(UnitsNet.RotationalSpeed.FromRevolutionsPerMinute(Random.Shared.Next(1000)));
            await Task.Delay(1.Seconds(), token);
        }
    });

    public async ValueTask SetPasLevel(PasLevel level, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1.Seconds(), cancellationToken);
        pasLevelSubject.OnNext(level);
    }

    public void Dispose()
    {
        pasLevelSubject.Dispose();
    }
}
