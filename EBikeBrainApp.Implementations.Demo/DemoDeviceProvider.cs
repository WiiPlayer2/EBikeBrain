using System.Reactive.Linq;
using System.Reactive.Subjects;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;
using LanguageExt;

namespace EBikeBrainApp.Implementations.Demo;

public class DemoDeviceProvider : IDeviceProvider
{
    public IConnectableObservable<Lst<Device>> Devices => Observable.Return(Prelude.List(
        new Device("Test", DeviceId.From("test")),
        new Device("Test 2", DeviceId.From("test-2"))
    )).Publish();
}
