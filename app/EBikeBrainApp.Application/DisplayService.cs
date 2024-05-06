using System.Reactive.Linq;
using EBikeBrainApp.Domain.Events;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class DisplayService<RT>(
    ConnectionService<RT> connectionService,
    ConfigurationService configurationService,
    LogService logService,
    IEventBus eventBus)
    where RT : struct, HasCancel<RT>
{
    private readonly IObservable<PasService<RT>> pasService = connectionService.BikeMotorConnection
        .Select(x => new PasService<RT>(x));

    private readonly IObservable<SpeedService<RT>> speedService = connectionService.BikeMotorConnection
        .Select(x => new SpeedService<RT>(x, configurationService, eventBus));

    public IObservable<Percentage> Battery { get; } = connectionService.BikeMotorConnection
        .Select(x => x.Battery)
        .Switch();

    public IObservable<bool> CanConnectBike => connectionService.CanConnect;

    public IObservable<bool> CanDisconnectBike => connectionService.CanDisconnect;

    public IObservable<ElectricCurrent> Current { get; } = connectionService.BikeMotorConnection
        .Select(x => x.Current)
        .Switch();

    public IObservable<Option<Aff<RT, Unit>>> DecreasePasLevelCommand { get; }

    public IObservable<Option<Aff<RT, Unit>>> IncreasePasLevelCommand { get; }

    public IObservable<Lst<LogEntry>> LogEntries => logService.LogEntries;

    public IObservable<Option<PasLevel>> PasLevel => pasService
        .Select(x => x.Current.Select(x => x.ToOption()))
        .Switch();

    public IObservable<Power> Power => Current
        .CombineLatest(configurationService.Bike.Select(x => x.MotorVoltage).Distinct())
        .Select(t => t.First * t.Second.Value);

    public IObservable<WheelRotationalSpeed> RotationalSpeed { get; } = eventBus
        .GetStream<WheelRotationalSpeed>();

    // public IObservable<BikeSpeed> Speed { get; } = eventBus
    //     .GetStream<BikeSpeed>();
    public IObservable<BikeSpeed> Speed => speedService
        .Select(x => x.Speed)
        .Switch()
        .Select(x => BikeSpeed.From((Speed) x));

    public void Connect() => connectionService.BikeMotorConnection.Connect();

    public void Disconnect() => throw new NotImplementedException();
}
