using System.Reactive;
using System.Reactive.Linq;
using LanguageExt.Common;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

[Obsolete]
public class BikeMotorService<RT>(
    IBikeMotor bikeMotor)
    where RT : struct, HasCancel<RT>
{
    public IObservable<Fin<PasLevel>> PasLevel { get; }
        = WrapErrors(bikeMotor.PasLevel, () => "Failed to read PAS level");

    public Aff<RT, Unit> SetPassLevel(PasLevel level) =>
        Aff((RT rt) => bikeMotor.SetPasLevel(level, rt.CancellationToken).ToUnit());

    private static IObservable<Fin<T>> WrapErrors<T>(IObservable<T> observable, Func<string> getError) => observable
        .Materialize()
        .Retry()
        .Where(x => x.Kind != NotificationKind.OnCompleted)
        .Select(x => x.HasValue
            ? FinSucc(x.Value)
            : FinFail<T>(x.Exception is not null
                ? Error.New(x.Exception)
                : Error.New(getError())));
}
