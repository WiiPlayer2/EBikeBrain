using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class BikeService<RT> where RT : struct, HasCancel<RT>
{
    private readonly BehaviorSubject<PasLevel> lastPasLevel = new(PasLevel.Level1);

    public BikeService(BikeMotorService<RT> bikeMotorService,
        Length wheelDiameter)
    {
        this.Motor = bikeMotorService;

        Speed = bikeMotorService.RotationalSpeed
            .Select(x => x.Map(x => x.ToLinearSpeed(wheelDiameter)));

        CanDecreasePasLevel = lastPasLevel
            .Select(x => x.CanDecrease());

        CanIncreasePasLevel = lastPasLevel
            .Select(x => x.CanIncrease());

        bikeMotorService.PasLevel
            .SelectMany(x => x)
            .Subscribe(lastPasLevel);
    }

    public IObservable<bool> CanDecreasePasLevel { get; }

    public IObservable<bool> CanIncreasePasLevel { get; }

    public BikeMotorService<RT> Motor { get; }

    public IObservable<Fin<Speed>> Speed { get; }

    public Aff<RT, Unit> DecreasePasLevel() =>
        from nextLevel in lastPasLevel.Value.TryDecrease()
            .ToEff("PAS level can't be decreased")
        from _ in Motor.SetPassLevel(nextLevel)
        select unit;

    public Aff<RT, Unit> IncreasePasLevel() =>
        from nextLevel in lastPasLevel.Value.TryIncrease()
            .ToEff("PAS level can't be increased")
        from _ in Motor.SetPassLevel(nextLevel)
        select unit;
}
