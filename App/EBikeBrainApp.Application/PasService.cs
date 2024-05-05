using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class PasService<RT> : IDisposable
    where RT : struct, HasCancel<RT>
{
    private readonly BikeMotorService<RT> bikeMotorService;

    private readonly BehaviorSubject<PasLevel> pasLevelSubject = new(PasLevel.Level1);

    private readonly CompositeDisposable subscriptions;

    public PasService(BikeMotorService<RT> bikeMotorService)
    {
        this.bikeMotorService = bikeMotorService;

        subscriptions = new CompositeDisposable(
            bikeMotorService.PasLevel
                // .Do() // TODO: log error or maybe not as the value is passed through
                .SelectMany(x => x)
                .Subscribe(pasLevelSubject)
        );

        DecreaseCommand = pasLevelSubject
            .Select(level => level.CanDecrease()
                ? DecreasePasLevel()
                : Option<Aff<RT, Unit>>.None);

        IncreaseCommand = pasLevelSubject
            .Select(level => level.CanIncrease()
                ? IncreasePasLevel()
                : Option<Aff<RT, Unit>>.None);
    }

    public IObservable<Fin<PasLevel>> Current => bikeMotorService.PasLevel;

    public IObservable<Option<Aff<RT, Unit>>> DecreaseCommand { get; }

    public IObservable<Option<Aff<RT, Unit>>> IncreaseCommand { get; }

    public void Dispose()
    {
        subscriptions.Dispose();
        pasLevelSubject.Dispose();
    }

    private Aff<RT, Unit> DecreasePasLevel() =>
        from nextLevel in pasLevelSubject.Value.TryDecrease()
            .ToEff("PAS level can't be decreased")
        from _ in bikeMotorService.SetPassLevel(nextLevel)
        select unit;

    private Aff<RT, Unit> IncreasePasLevel() =>
        from nextLevel in pasLevelSubject.Value.TryIncrease()
            .ToEff("PAS level can't be increased")
        from _ in bikeMotorService.SetPassLevel(nextLevel)
        select unit;
}
