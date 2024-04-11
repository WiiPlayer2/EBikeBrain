using System.Reactive.Linq;
using System.Reactive.Subjects;
using Android.Bluetooth;
using Android.Util;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;
using LanguageExt;
using LanguageExt.UnitsOfMeasure;
using Microsoft.Maui.ApplicationModel;

namespace EBikeBrainApp.Implementations.Android;

public class AndroidDeviceProvider(IMainThreadDispatcher mainThreadDispatcher) : IDeviceProvider
{
    public IConnectableObservable<Lst<Device>> Devices { get; } = Observable.Create<Lst<Device>>(async (observer, token) =>
        {
            try
            {
                Log.Debug("EBike", "Checking permissions");

                var bluetoothPermission = new Permissions.Bluetooth();
                if (await mainThreadDispatcher.Invoke(() => bluetoothPermission.RequestAsync()) != PermissionStatus.Granted)
                    throw new Exception("Bluetooth permission not granted.");

                var adapter = BluetoothAdapter.DefaultAdapter ?? throw new Exception("No Bluetooth available");

                while (!token.IsCancellationRequested)
                {
                    Log.Debug("EBike", "Getting devices");
                    observer.OnNext(Prelude.toList(
                        from bondedDevice in adapter.BondedDevices ?? Array.Empty<BluetoothDevice>()
                        let deviceId = DeviceId.From(bondedDevice.Address ?? throw new InvalidOperationException())
                        let deviceName = bondedDevice.Name ?? throw new InvalidOperationException()
                        let device = new Device(deviceName, deviceId)
                        select device));

                    await Task.Delay(10.Seconds());
                }
            }
            catch (Exception e)
            {
                Log.Error("EBike", $"Error while getting devices. {e}");
            }
        })
        .Publish();
}
