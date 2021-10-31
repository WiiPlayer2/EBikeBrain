using Android.Bluetooth;
using Android.Content;
using EBikeBrain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Java.Util;
using Microsoft.Maui;

namespace EBikeBrain
{
    internal class MainViewModel
    {
        private BluetoothSocket? currentSocket;

        public ActionCommand ConnectCommand { get; }

        public ActionCommand DisconnectCommand { get; }

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
            }
            catch
            {
                currentSocket = null;
            }

            ConnectCommand.Refresh();
            DisconnectCommand.Refresh();
        }

        private void Disconnect()
        {
            currentSocket?.Close();
            currentSocket = null;

            ConnectCommand.Refresh();
            DisconnectCommand.Refresh();
        }
    }
}
