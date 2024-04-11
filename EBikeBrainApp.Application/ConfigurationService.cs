using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EBikeBrainApp.Application;

public class ConfigurationService : IDisposable
{
    private readonly BehaviorSubject<BikeConnectionConfiguration> connection = new(new BikeConnectionConfiguration(new Device("Test", DeviceId.From("test"))));

    public IObservable<BikeConfiguration> Bike { get; } = Observable.Return(new BikeConfiguration(WheelDiameter.From(Length.FromInches(28))));

    public ISubject<BikeConnectionConfiguration> Connection => connection;

    public void Dispose()
    {
        connection.Dispose();
    }
}