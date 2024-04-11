using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class ConnectionService<RT>(
    IBikeMotorConnector bikeMotorConnector,
    ConfigurationService configurationService
)
    where RT : struct, HasCancel<RT>
{
    public IConnectableObservable<BikeMotorService<RT>> BikeMotorConnection { get; } = configurationService.Connection
        .Select(config => CreateBikeMotorObservable(bikeMotorConnector, config.Device.Id))
        .Switch()
        .Select(x => new BikeMotorService<RT>(x))
        .Publish();

    public IObservable<bool> CanConnect { get; } = bikeMotorConnector.IsBusy.Select(x => !x);

    public IObservable<bool> CanDisconnect { get; } = Observable.Return(false);

    private static IObservable<IBikeMotor> CreateBikeMotorObservable(IBikeMotorConnector bikeMotorConnector, DeviceId deviceId) =>
        Observable.Create<IBikeMotor>(async (observer, token) =>
        {
            var bikeMotor = await bikeMotorConnector.ConnectDevice(deviceId, token);
            observer.OnNext(bikeMotor);
            return bikeMotor as IDisposable ?? Disposable.Empty;
        });
}
