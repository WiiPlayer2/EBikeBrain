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
        private const double WHEEL_SIZE_IN_METERS = 0.7112; // 28"

        private BluetoothSocket? currentSocket;

        private double currentRpm;

        private BikeComm.PasLevel currentLevel;

        private BikeComm? bikeComm;

        private CancellationTokenSource? currentCancellationTokenSource;

        private Task? updateLoopTask;

        public ActionCommand ConnectCommand { get; }

        public ActionCommand DisconnectCommand { get; }

        public ActionCommand IncreaseLevelCommand { get; }

        public ActionCommand DecreaseLevelCommand { get; }

        public double CurrentRPM
        {
            get => currentRpm;
            set
            {
                currentRpm = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentSpeed));
            }
        }

        public double CurrentSpeed => CurrentRPM * WHEEL_SIZE_IN_METERS * 60.0 / 1000.0;

        public BikeComm.PasLevel CurrentLevel
        {
            get => currentLevel;
            set
            {
                currentLevel = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ConnectCommand = new ActionCommand(Connect, () => currentSocket == null);
            DisconnectCommand = new ActionCommand(Disconnect, () => currentSocket?.IsConnected ?? false);
            IncreaseLevelCommand = new ActionCommand(IncreaseLevel, () => bikeComm is {IsBusy: false} && CurrentLevel != BikeComm.PasLevel.PAS9);
            DecreaseLevelCommand = new ActionCommand(DecreaseLevel, () => bikeComm is {IsBusy: false} && CurrentLevel != BikeComm.PasLevel.PAS0);
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
                await bikeComm.SetPasLevel(BikeComm.PasLevel.PAS0);
                await bikeComm.SetMaxWheelRpm(186); // 25km/h with 28" wheel
                await bikeComm.SetLights(false);
                updateLoopTask = Task.Run(UpdateLoop);
            }
            catch
            {
                currentSocket = null;
                if (bikeComm != null)
                {
                    await bikeComm.DisposeAsync();
                    bikeComm = null;
                }
            }

            ConnectCommand.Refresh();
            DisconnectCommand.Refresh();
            IncreaseLevelCommand.Refresh();
            DecreaseLevelCommand.Refresh();
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

                await bikeComm!.SetPasLevel(CurrentLevel);
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
            CurrentLevel = BikeComm.PasLevel.PAS0;

            try
            {
                await (updateLoopTask ?? Task.CompletedTask);
            }
            catch { }

            ConnectCommand.Refresh();
            DisconnectCommand.Refresh();
            IncreaseLevelCommand.Refresh();
            DecreaseLevelCommand.Refresh();
        }

        private Task IncreaseLevel()
            => SetLevel(CurrentLevel switch
            {
                BikeComm.PasLevel.PAS0 => BikeComm.PasLevel.PAS1,
                BikeComm.PasLevel.PAS1 => BikeComm.PasLevel.PAS2,
                BikeComm.PasLevel.PAS2 => BikeComm.PasLevel.PAS3,
                BikeComm.PasLevel.PAS3 => BikeComm.PasLevel.PAS4,
                BikeComm.PasLevel.PAS4 => BikeComm.PasLevel.PAS5,
                BikeComm.PasLevel.PAS5 => BikeComm.PasLevel.PAS6,
                BikeComm.PasLevel.PAS6 => BikeComm.PasLevel.PAS7,
                BikeComm.PasLevel.PAS7 => BikeComm.PasLevel.PAS8,
                BikeComm.PasLevel.PAS8 => BikeComm.PasLevel.PAS9,
            });

        private Task DecreaseLevel()
            => SetLevel(CurrentLevel switch
            {
                BikeComm.PasLevel.PAS1 => BikeComm.PasLevel.PAS0,
                BikeComm.PasLevel.PAS2 => BikeComm.PasLevel.PAS1,
                BikeComm.PasLevel.PAS3 => BikeComm.PasLevel.PAS2,
                BikeComm.PasLevel.PAS4 => BikeComm.PasLevel.PAS3,
                BikeComm.PasLevel.PAS5 => BikeComm.PasLevel.PAS4,
                BikeComm.PasLevel.PAS6 => BikeComm.PasLevel.PAS5,
                BikeComm.PasLevel.PAS7 => BikeComm.PasLevel.PAS6,
                BikeComm.PasLevel.PAS8 => BikeComm.PasLevel.PAS7,
                BikeComm.PasLevel.PAS9 => BikeComm.PasLevel.PAS8,
            });

        private async Task SetLevel(BikeComm.PasLevel level)
        {
            try
            {
                await bikeComm!.SetPasLevel(level);
                CurrentLevel = level;
            }
            finally
            {
                IncreaseLevelCommand.Refresh();
                DecreaseLevelCommand.Refresh();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
