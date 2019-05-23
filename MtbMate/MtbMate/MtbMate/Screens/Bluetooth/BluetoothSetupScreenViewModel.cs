using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MtbMate.Utilities;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
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
        public ObservableCollection<DeviceInfo> DevicesFound { get; }
        public bool IsBluetoothOn => ble.IsOn;
        public bool CanStartScanning => IsBluetoothOn && !IsScanning;

        public BluetoothSetupScreenViewModel()
        {
            DevicesFound = new ObservableCollection<DeviceInfo>();

            adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
            adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed;
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
            var deviceInfo = DependencyService.Get<IDeviceInfo>().GetDeviceInfo(e.Device);

            DevicesFound.Add(deviceInfo);

            Debug.WriteLine($"Device found: {deviceInfo.DisplayName}");
        }

        private void Adapter_ScanTimeoutElapsed(object sender, EventArgs e)
        {
            OnPropertyChanged();

            Debug.WriteLine("Scanning timed out...");
        }

        private void Ble_StateChanged(object sender, BluetoothStateChangedArgs e)
        {
            OnPropertyChanged();
        }

        public async Task TryStartScanning()
        {
            IsScanning = true;

            await adapter.StartScanningForDevicesAsync();

            IsScanning = false;
        }

        public async Task ConnectToDevice(IDevice device)
        {
            try
            {
                await adapter.ConnectToDeviceAsync(device);
            }
            catch (DeviceConnectionException e)
            {
                // ... could not connect to device
                Debug.WriteLine(e);
            }
        }
    }
}
