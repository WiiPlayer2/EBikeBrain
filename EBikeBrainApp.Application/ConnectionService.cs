using System.Reactive.Linq;
using System.Reactive.Subjects;
using EBikeBrainApp.Application.Abstractions;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class ConnectionService<RT>(
    IBikeMotor bikeMotor
)
    where RT : struct, HasCancel<RT>
{
    public IConnectableObservable<BikeMotorService<RT>> BikeMotorConnection { get; } = Observable.Return(new BikeMotorService<RT>(bikeMotor)).Publish();

    public IObservable<bool> CanConnect { get; } = Observable.Return(true);

    public IObservable<bool> CanDisconnect { get; } = Observable.Return(false);
}
