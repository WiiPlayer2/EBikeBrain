using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions.Commands;
using EBikeBrainApp.Application.Abstractions.Events;
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
    private readonly IObservable<BikeMotorService<RT>> bikeMotorService = eventBus
        .GetStream<BikeMotorConnected>()
        .Select(x => new BikeMotorService<RT>(x.BikeMotor));

    public IObservable<Percentage> Battery => bikeMotorService
        .Select(x => x.Battery)
        .Switch();

    public IObservable<bool> CanConnectBike => connectionService.CanConnect;

    public IObservable<bool> CanDisconnectBike => connectionService.CanDisconnect;

    public IObservable<ElectricCurrent> Current => bikeMotorService
        .Select(x => x.Current)
        .Switch();

    public IObservable<Option<Aff<RT, Unit>>> DecreasePasLevelCommand { get; }

    public IObservable<Option<Aff<RT, Unit>>> IncreasePasLevelCommand { get; }

    public IObservable<Lst<LogEntry>> LogEntries => logService.LogEntries;

    public IObservable<Option<PasLevel>> PasLevel => pasService
        .Select(x => x.Current.Select(x => x.ToOption()))
        .Switch();

    private IObservable<PasService<RT>> pasService => bikeMotorService
        .Select(x => new PasService<RT>(x));

    public IObservable<Power> Power => Current
        .CombineLatest(configurationService.Bike.Select(x => x.MotorVoltage).Distinct())
        .Select(t => t.First * t.Second.Value);

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
