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

        SelectedWheelDiameter = configurationService.Bike
            .Select(x => x.WheelDiameter.Value.Value)
            .DistinctUntilChanged()
            .ToReactiveProperty();

        SelectedMotorVoltage = configurationService.Bike
            .Select(x => x.MotorVoltage.Value.Value)
            .DistinctUntilChanged()
            .ToReactiveProperty();

        subscriptions = new CompositeDisposable(
            SelectedDevice
                .Where(x => x is not null)
                .CombineLatest(configurationService.Connection, (device, configuration) => configuration with {Device = device!})
                .DistinctUntilChanged()
                .Subscribe(configurationService.Connection),
            SelectedWheelDiameter
                .CombineLatest(SelectedMotorVoltage, configurationService.Bike, (wheelDiameter, motorVoltage, config) => config with
                {
                    WheelDiameter = WheelDiameter.From(Length.FromInches(wheelDiameter)),
                    MotorVoltage = MotorVoltage.From(ElectricPotential.FromVolts(motorVoltage)),
                })
                .DistinctUntilChanged()
                .Subscribe(configurationService.Bike)
        );
    }

    public IObservable<Lst<Device>> Devices => deviceProvider1.Devices.RefCount();

    public ReactiveProperty<Device?> SelectedDevice { get; }

    public ReactiveProperty<double> SelectedMotorVoltage { get; }

    public ReactiveProperty<double> SelectedWheelDiameter { get; }

    public void Dispose()
    {
        subscriptions.Dispose();
        SelectedDevice.Dispose();
    }
}
