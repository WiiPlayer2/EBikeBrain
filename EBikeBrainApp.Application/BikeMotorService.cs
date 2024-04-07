using System.Reactive;
using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions;
using LanguageExt.Common;

namespace EBikeBrainApp.Application;

public class BikeMotorService(
    IBikeMotor bikeMotor)
{
    public IObservable<Fin<RotationalSpeed>> RotationalSpeed { get; } = bikeMotor.RotationalSpeed
        .Materialize()
        .Retry()
        .Where(x => x.Kind != NotificationKind.OnCompleted)
        .Select(x => x.HasValue
            ? FinSucc(x.Value)
            : FinFail<RotationalSpeed>(x.Exception is not null
                ? Error.New(x.Exception)
                : Error.New("Failed to get rotational speed.")));
}
