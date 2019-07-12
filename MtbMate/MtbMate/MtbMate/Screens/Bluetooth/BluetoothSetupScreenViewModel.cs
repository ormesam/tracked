using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Utilities;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Forms;

namespace MtbMate.Screens.Bluetooth
{
    public class BluetoothSetupScreenViewModel : ViewModelBase
    {
        private bool isScanning;
        private IBluetoothLE ble => CrossBluetoothLE.Current;
        private IAdapter adapter => CrossBluetoothLE.Current.Adapter;
        public ObservableCollection<IDevice> DevicesFound { get; }
        public bool IsBluetoothOn => ble.IsOn;
        public bool ShowDeviceList => IsBluetoothOn && !IsDeviceConnected;
        public bool CanStartScanning => IsBluetoothOn && !IsDeviceConnected && !IsScanning;
        public bool IsDeviceConnected => ble.Adapter.ConnectedDevices.Any();

        public BluetoothSetupScreenViewModel(MainContext context) : base(context)
        {
            DevicesFound = new ObservableCollection<IDevice>();

            adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
            adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed;
            adapter.DeviceConnected += Adapter_DeviceConnected;
            ble.StateChanged += Ble_StateChanged;
        }

        public bool IsScanning {
            get { return isScanning; }
            set {
                if (isScanning != value)
                {
                    isScanning = value;
                    OnPropertyChanged(nameof(IsScanning));
                    OnPropertyChanged(nameof(CanStartScanning));
                }
            }
        }

        private void Adapter_DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Device.Name))
            {
                return;
            }

            DevicesFound.Add(e.Device);

            Debug.WriteLine($"Device found: {e.Device.Name}");
        }

        private void Adapter_ScanTimeoutElapsed(object sender, EventArgs e)
        {
            OnPropertyChanged();
        }

        private void Adapter_DeviceConnected(object sender, DeviceEventArgs e)
        {
            OnPropertyChanged();
        }

        private void Characteristic_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            var temp = e.Characteristic.StringValue;

            Debug.WriteLine(DateTime.Now + " - " + temp);
        }

        private void Ble_StateChanged(object sender, BluetoothStateChangedArgs e)
        {
            OnPropertyChanged();
        }

        public async Task TryStartScanning()
        {
            DevicesFound.Clear();

            IsScanning = true;

            await adapter.StartScanningForDevicesAsync();

            IsScanning = false;
        }

        public async Task ConnectToDevice(IDevice device)
        {
            try
            {
                await adapter.StopScanningForDevicesAsync();

                IsScanning = false;

                await adapter.ConnectToDeviceAsync(device);

                DevicesFound.Clear();
            }
            catch (DeviceConnectionException e)
            {
                // ... could not connect to device
                Debug.WriteLine(e);
            }
        }

        public async Task DisconnectDevice()
        {
            foreach (var device in adapter.ConnectedDevices)
            {
                await adapter.DisconnectDeviceAsync(device);
            }

            OnPropertyChanged();
        }
    }
}
