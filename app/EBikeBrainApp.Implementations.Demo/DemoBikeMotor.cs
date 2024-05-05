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

    public IObservable<Percentage> Battery { get; } = Observable.Create<Percentage>(async (observer, token) =>
    {
        while (!token.IsCancellationRequested)
        {
            observer.OnNext(Percentage.From(Random.Shared.Next(1000) / 10.0));
            await Task.Delay(1.Seconds(), token);
        }
    });

    public IObservable<ElectricCurrent> Current { get; } = Observable.Create<ElectricCurrent>(async (observer, token) =>
    {
        while (!token.IsCancellationRequested)
        {
            observer.OnNext(ElectricCurrent.FromAmperes(Random.Shared.Next(255)));
            await Task.Delay(1.Seconds(), token);
        }
    });

    public IObservable<PasLevel> PasLevel => pasLevelSubject;

    public IObservable<RotationalSpeed> RotationalSpeed { get; } = Observable.Create<RotationalSpeed>(async (observer, token) =>
    {
        while (!token.IsCancellationRequested)
        {
            observer.OnNext(UnitsNet.RotationalSpeed.FromRevolutionsPerMinute(Random.Shared.Next(255)));
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
