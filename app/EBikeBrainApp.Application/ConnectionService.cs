using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

[Obsolete]
public class ConnectionService<RT>(
    IBikeMotorConnector bikeMotorConnector,
    ConfigurationService configurationService,
    IEventStream<LogEntry> logEvents)
    where RT : struct, HasCancel<RT>
{
    public IConnectableObservable<BikeMotorService<RT>> BikeMotorConnection { get; } = configurationService.Connection
        .SelectMany(x => x.Device)
        .Select(device => CreateBikeMotorObservable(bikeMotorConnector, device.Id, logEvents))
        .Switch()
        .Select(x => new BikeMotorService<RT>(x))
        .Publish();

    public IObservable<bool> CanConnect { get; } = bikeMotorConnector.IsBusy.Select(x => !x);

    public IObservable<bool> CanDisconnect { get; } = Observable.Return(false);

    private static IObservable<IBikeMotor> CreateBikeMotorObservable(IBikeMotorConnector bikeMotorConnector, DeviceId deviceId, IEventStream<LogEntry> logEntries) =>
        Observable.Create<IBikeMotor>(async (observer, token) =>
        {
            logEntries.Publish(LogEntry.From($"Connecting to {deviceId}..."));
            var bikeMotor = await bikeMotorConnector.ConnectDevice(deviceId, token);
            logEntries.Publish(LogEntry.From($"Connected to {deviceId}."));
            observer.OnNext(bikeMotor);
            return bikeMotor as IDisposable ?? Disposable.Empty;
        });
}
