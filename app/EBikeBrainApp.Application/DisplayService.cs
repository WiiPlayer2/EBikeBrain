using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions.Commands;
using EBikeBrainApp.Domain.Events;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class DisplayService<RT>(
    ConnectionService<RT> connectionService,
    ConfigurationService configurationService,
    LogService logService,
    IEventBus eventBus,
    ICommandPublisher<ConnectDevice> connectDevicePublisher)
    where RT : struct, HasCancel<RT>
{
    public IObservable<BikeMotorBatteryPercentage> Battery => eventBus
        .GetStream<BikeMotorBatteryPercentage>();

    public IObservable<bool> CanConnectBike => connectionService.CanConnect;

    public IObservable<bool> CanDisconnectBike => connectionService.CanDisconnect;

    public IObservable<Option<Aff<RT, Unit>>> DecreasePasLevelCommand { get; }

    public IObservable<Option<Aff<RT, Unit>>> IncreasePasLevelCommand { get; }

    public IObservable<Lst<LogEntry>> LogEntries => logService.LogEntries;

    public IObservable<BikeMotorPasLevel> PasLevel => eventBus
        .GetStream<BikeMotorPasLevel>();

    public IObservable<BikeMotorPower> Power => eventBus
        .GetStream<BikeMotorPower>();

    public IObservable<WheelRotationalSpeed> RotationalSpeed { get; } = eventBus
        .GetStream<WheelRotationalSpeed>();

    public IObservable<BikeSpeed> Speed { get; } = eventBus
        .GetStream<BikeSpeed>();

    public async void Connect()
    {
        var device = await configurationService.Connection
            .SelectMany(x => x.Device)
            .FirstOrDefaultAsync();

        if (device is null)
            return;

        await connectDevicePublisher.Publish(new ConnectDevice(device));
    }

    public void Disconnect() => throw new NotImplementedException();
}
