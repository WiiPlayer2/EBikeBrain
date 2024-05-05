using System.Reactive.Subjects;

namespace EBikeBrainApp.Application.Abstractions;

public interface IDeviceProvider
{
    IConnectableObservable<Lst<Device>> Devices { get; }
}
