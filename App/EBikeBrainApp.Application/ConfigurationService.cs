using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EBikeBrainApp.Application;

public class ConfigurationService : IDisposable
{
    private readonly CompositeDisposable subscriptions;

    public ConfigurationService(
        IConfigurationStore<BikeConnectionConfiguration> connectionConfigStore,
        IConfigurationStore<BikeConfiguration> bikeConfigStore)
    {
        (Connection, var connectionDisposables) = CreateConfigSubject(
            connectionConfigStore,
            () => new BikeConnectionConfiguration(None));

        (Bike, var bikeDisposables) = CreateConfigSubject(
            bikeConfigStore,
            () => new BikeConfiguration(
                WheelDiameter.From(Length.FromInches(28)),
                MotorVoltage.From(ElectricPotential.FromVolts(36))));

        subscriptions = new CompositeDisposable(
            connectionDisposables,
            bikeDisposables);
    }

    public ISubject<BikeConfiguration> Bike { get; }

    public ISubject<BikeConnectionConfiguration> Connection { get; }

    public void Dispose()
    {
        subscriptions.Dispose();
    }

    private static (ISubject<T> Subject, IDisposable Disposable) CreateConfigSubject<T>(
        IConfigurationStore<T> configurationStore,
        Func<T> getDefault)
    {
        var internalSubject = new Subject<T>();
        var observable = Observable
            .FromAsync(configurationStore.Load)
            .Select(x => x.IfNone(getDefault))
            .Concat(internalSubject);
        var subject = Subject.Create<T>(internalSubject, observable);

        var disposables = new CompositeDisposable(
            internalSubject
                .SelectMany(x => Observable.FromAsync(token => configurationStore.Store(x, token)))
                .Subscribe(_ => { }),
            internalSubject
        );

        return (subject, disposables);
    }
}
