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
using Microsoft.Maui.ApplicationModel;

namespace EBikeBrain
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private const double WHEEL_SIZE_IN_METERS = 0.7112; // 28"

        private const double BATTERY_VOLTAGE = 36;

        private BluetoothSocket? currentSocket;

        private double currentRpm;

        private BikeComm.PasLevel currentLevel;

        private BikeComm? bikeComm;

        private CancellationTokenSource? currentCancellationTokenSource;

        private Task? updateLoopTask;

        private double currentBatteryPercentage;

        private double currentAmps;

        public ActionCommand ConnectCommand { get; }

        public ActionCommand DisconnectCommand { get; }

        public ActionCommand IncreaseLevelCommand { get; }

        public ActionCommand DecreaseLevelCommand { get; }

        public ActionCommand FlushBufferCommand { get; }

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

        public double CurrentSpeed => 3600.0 / 1000.0 * WHEEL_SIZE_IN_METERS * Math.PI / 60.0 * CurrentRPM;

        public double CurrentBatteryPercentage
        {
            get => currentBatteryPercentage;
            set
            {
                currentBatteryPercentage = value;
                OnPropertyChanged();
            }
        }

        public double CurrentAmps
        {
            get => currentAmps;
            set
            {
                currentAmps = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentPower));
            }
        }

        public double CurrentPower => BATTERY_VOLTAGE * CurrentAmps;

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
            FlushBufferCommand = new ActionCommand(FlushBuffer, () => bikeComm != null);
            IncreaseLevelCommand = new ActionCommand(IncreaseLevel, () => bikeComm is {IsBusy: false} && CurrentLevel != BikeComm.PasLevel.PAS9);
            DecreaseLevelCommand = new ActionCommand(DecreaseLevel, () => bikeComm is {IsBusy: false} && CurrentLevel != BikeComm.PasLevel.PAS0);
        }

        private void RefreshCommands()
        {
            ConnectCommand.Refresh();
            DisconnectCommand.Refresh();
            FlushBufferCommand.Refresh();
            IncreaseLevelCommand.Refresh();
            DecreaseLevelCommand.Refresh();
        }

        private async Task FlushBuffer()
        {
            try
            {
                await bikeComm!.FlushBuffer();
            }
            catch (Exception ex)
            {
            }
        }

        private async Task Connect()
        {
            var bluetoothPermission = new Permissions.Bluetooth();
            if (await bluetoothPermission.CheckStatusAsync() != PermissionStatus.Granted && await bluetoothPermission.RequestAsync() != PermissionStatus.Granted)
            {
                throw new Exception("Permission not granted");
            }

            var bluetoothManager = (BluetoothManager?) MauiApplication.Current.GetSystemService(MauiApplication.BluetoothService);
            var adapter = bluetoothManager?.Adapter;
            var device = adapter?.GetRemoteDevice("94:B9:7E:D4:28:E2");
            var bluetoothServiceClass = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
            currentSocket = device?.CreateRfcommSocketToServiceRecord(bluetoothServiceClass);

            if (currentSocket == null)
                return;

            try
            {
                RefreshCommands();
                await currentSocket.ConnectAsync();
                bikeComm = new BikeComm(currentSocket.InputStream!, currentSocket.OutputStream!);

                IncreaseLevelCommand.Add(bikeComm);
                DecreaseLevelCommand.Add(bikeComm);

                await bikeComm.SetPasLevel(BikeComm.PasLevel.PAS0);
                await bikeComm.SetMaxWheelRpm(186); // 25km/h with 28" wheel
                await bikeComm.SetLights(false);
                updateLoopTask = Task.Run(UpdateLoop);
            }
            catch(Exception ex)
            {
                currentSocket.Close();
                currentSocket.Dispose();
                currentSocket = null;
                if (bikeComm != null)
                {
                    await bikeComm.DisposeAsync();
                    bikeComm = null;
                }
            }

            RefreshCommands();
        }

        private async Task UpdateLoop()
        {
            currentCancellationTokenSource?.Dispose();
            currentCancellationTokenSource = new();

            while (!currentCancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(500);
                if (currentCancellationTokenSource.IsCancellationRequested)
                    return;

                try
                { 
                    await bikeComm!.SetPasLevel(CurrentLevel);
                    CurrentRPM = await bikeComm!.GetWheelRpm();
                    CurrentBatteryPercentage = await bikeComm!.GetBatteryPercentage();
                    CurrentAmps = await bikeComm!.GetAmps();
                }
                catch
                {
                    await bikeComm!.FlushBuffer();
                }
            }
        }

        private async Task Disconnect()
        {
            currentCancellationTokenSource?.Cancel();

            CurrentLevel = BikeComm.PasLevel.PAS0;
            try
            {
                await bikeComm!.SetPasLevel(BikeComm.PasLevel.PAS0);
            }
            catch
            {
            }

            await bikeComm!.DisposeAsync();

            IncreaseLevelCommand.Remove(bikeComm);
            DecreaseLevelCommand.Remove(bikeComm);

            currentSocket?.Close();
            currentSocket = null;
            bikeComm = null;

            try
            {
                await (updateLoopTask ?? Task.CompletedTask);
            }
            catch { }

            RefreshCommands();
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
                RefreshCommands();
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
