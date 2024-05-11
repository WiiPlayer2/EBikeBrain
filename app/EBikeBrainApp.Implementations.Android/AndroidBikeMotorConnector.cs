using Android.Bluetooth;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;
using EBikeBrainApp.Protocols.Bafang;
using EBikeBrainApp.Utils;
using Java.Util;
using Microsoft.Extensions.Logging;

namespace EBikeBrainApp.Implementations.Android;

public class AndroidBikeMotorConnector(BluetoothAdapter bluetoothAdapter, ILogger<ProtocolInterceptorBikeMotor> logger) : IBikeMotorConnector, IDisposable
{
    private static readonly UUID RF_COMM_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB")!;

    private readonly Busy connectingBusy = new();

    public Task<IBikeMotor> ConnectDevice(DeviceId deviceId, CancellationToken cancellationToken = default) => connectingBusy.Run(() => Task.Run<IBikeMotor>(async () =>
    {
        var bluetoothDevice = bluetoothAdapter.GetRemoteDevice(deviceId.Value);
        if (bluetoothDevice is null)
            throw new InvalidOperationException($"Failed to get bluetooth device {deviceId.Value}");

        var socket = bluetoothDevice.CreateRfcommSocketToServiceRecord(RF_COMM_UUID);
        if (socket is null)
            throw new InvalidOperationException("Failed to create RFcomm socket.");

        await socket.ConnectAsync();
        var bikeMotor = new ProtocolInterceptorBikeMotor(socket.InputStream!, logger);
        return bikeMotor;
    }, cancellationToken), cancellationToken);

    public IObservable<bool> IsBusy => connectingBusy.IsBusy;

    public void Dispose()
    {
        connectingBusy.Dispose();
    }
}
