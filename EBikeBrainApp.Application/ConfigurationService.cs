using System.Reactive.Linq;

namespace EBikeBrainApp.Application;

public class ConfigurationService
{
    public IObservable<BikeConfiguration> Bike { get; } = Observable.Return(new BikeConfiguration(WheelDiameter.From(Length.FromInches(28))));

    public IObservable<BikeConnectionConfiguration> Connection { get; } = Observable.Return(new BikeConnectionConfiguration(new Device("Test", DeviceId.From("test"))));
}
