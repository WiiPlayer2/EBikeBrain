using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EBikeBrainApp.Application;

public class ConfigurationService : IDisposable
{
    private readonly Subject<BikeConnectionConfiguration> connectionSubject = new();

    private readonly CompositeDisposable subscriptions;

    public ConfigurationService(IConfigurationStore configurationStore)
    {
        var connectionObservable = Observable
            .FromAsync(configurationStore.LoadConnectionConfig)
            .Select(x => x.IfNone(() => new BikeConnectionConfiguration(None)))
            .Concat(connectionSubject);
        Connection = Subject.Create<BikeConnectionConfiguration>(connectionSubject, connectionObservable);

        subscriptions = new CompositeDisposable(
            connectionSubject
                .SelectMany(x => Observable.FromAsync(token => configurationStore.StoreConnectionConfig(x, token)))
                .Subscribe(_ => { })
        );
    }

    public IObservable<BikeConfiguration> Bike { get; } = Observable.Return(new BikeConfiguration(
        WheelDiameter.From(Length.FromInches(28)),
        MotorVoltage.From(ElectricPotential.FromVolts(36))));

    public ISubject<BikeConnectionConfiguration> Connection { get; }

    public void Dispose()
    {
        subscriptions.Dispose();
        connectionSubject.Dispose();
    }
}
