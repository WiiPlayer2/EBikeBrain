using Android.Bluetooth;
using Android.Content;
using EBikeBrain.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using EBikeBrain.Annotations;
using Java.Util;
using Microsoft.Maui;

namespace EBikeBrain
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private BluetoothSocket? currentSocket;

        private double currentRpm;

        private BikeComm? bikeComm;

        private CancellationTokenSource? currentCancellationTokenSource;

        private Task? updateLoopTask;

        public ActionCommand ConnectCommand { get; }

        public ActionCommand DisconnectCommand { get; }

        public double CurrentRPM
        {
            get => currentRpm;
            set
            {
                currentRpm = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ConnectCommand = new ActionCommand(Connect, () => currentSocket == null);
            DisconnectCommand = new ActionCommand(Disconnect, () => currentSocket?.IsConnected ?? false);
        }

        private async Task Connect()
        {
            var bluetoothManager = (BluetoothManager?) MauiApplication.Current.GetSystemService(MauiApplication.BluetoothService);
            var adapter = bluetoothManager?.Adapter;
            var device = adapter?.GetRemoteDevice("C8:C9:A3:D2:E1:CE");
            var bluetoothServiceClass = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
            currentSocket = device?.CreateRfcommSocketToServiceRecord(bluetoothServiceClass);

            if (currentSocket == null)
                return;

            try
            {
                ConnectCommand.Refresh();
                await currentSocket.ConnectAsync();
                bikeComm = new BikeComm(currentSocket.InputStream!, currentSocket.OutputStream!);
                updateLoopTask = Task.Run(UpdateLoop);
            }
            catch
            {
                currentSocket = null;
            }

            ConnectCommand.Refresh();
            DisconnectCommand.Refresh();
        }

        private async Task UpdateLoop()
        {
            currentCancellationTokenSource?.Dispose();
            currentCancellationTokenSource = new();
            while (!currentCancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(250);
                if (currentCancellationTokenSource.IsCancellationRequested)
                    return;

                CurrentRPM = await bikeComm!.GetWheelRpm();
            }
        }

        private async Task Disconnect()
        {
            currentCancellationTokenSource?.Cancel();

            await bikeComm!.DisposeAsync();
            currentSocket?.Close();
            currentSocket = null;
            bikeComm = null;

            try
            {
                await (updateLoopTask ?? Task.CompletedTask);
            }
            catch { }

            ConnectCommand.Refresh();
            DisconnectCommand.Refresh();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
