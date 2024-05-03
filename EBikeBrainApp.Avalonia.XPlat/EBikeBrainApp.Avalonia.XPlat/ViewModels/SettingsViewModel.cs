using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using EBikeBrainApp.Application;
using EBikeBrainApp.Application.Abstractions;
using Reactive.Bindings;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels;

public class SettingsViewModel : ViewModelBase, IDisposable
{
    private readonly IDeviceProvider deviceProvider1;

    private readonly CompositeDisposable subscriptions;

    public SettingsViewModel(ConfigurationService configurationService,
        IDeviceProvider deviceProvider)
    {
        deviceProvider1 = deviceProvider;

        SelectedDevice = configurationService.Connection
            .Select(x => x.Device.IfNoneUnsafe(default(Device?)))
            .DistinctUntilChanged()
            .ToReactiveProperty();

        subscriptions = new CompositeDisposable(
            SelectedDevice
                .Where(x => x is not null)
                .CombineLatest(configurationService.Connection, (device, configuration) => configuration with {Device = device!})
                .DistinctUntilChanged()
                .Subscribe(configurationService.Connection));
    }

    public IObservable<Lst<Device>> Devices => deviceProvider1.Devices.RefCount();

    public ReactiveProperty<Device?> SelectedDevice { get; }

    public void Dispose()
    {
        subscriptions.Dispose();
        SelectedDevice.Dispose();
    }
}
