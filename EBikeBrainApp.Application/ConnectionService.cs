using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using EBikeBrainApp.Application.Abstractions;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class ConnectionService<RT>(
    IBikeMotorConnector bikeMotorConnector
)
    where RT : struct, HasCancel<RT>
{
    public IConnectableObservable<BikeMotorService<RT>> BikeMotorConnection { get; } = Observable.Create<IBikeMotor>(async (observer, token) =>
        {
            var bikeMotor = await bikeMotorConnector.ConnectDevice(DeviceId.From("test"), token);
            observer.OnNext(bikeMotor);
            return Disposable.Empty;
        })
        .Select(x => new BikeMotorService<RT>(x))
        .Publish();

    public IObservable<bool> CanConnect { get; } = Observable.Return(true);

    public IObservable<bool> CanDisconnect { get; } = Observable.Return(false);
}
