using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using MtbMate.Utilities;
using Xamarin.Forms;

namespace MtbMate.Screens.Bluetooth
{
    public class BluetoothSetupScreenViewModel : ViewModelBase
    {
        private DeviceInfo connectedDevice;
        public ObservableCollection<DeviceInfo> Devices { get; set; }
        public IBluetoothUtility BluetoothUtility => DependencyService.Resolve<IBluetoothUtility>();

        public BluetoothSetupScreenViewModel()
        {
            Devices = new ObservableCollection<DeviceInfo>();
        }

        public void LoadDeviceList()
        {
            BluetoothUtility.TurnBluetoothOn();

            ConnectedDevice = BluetoothUtility.GetConnectedDevice();

            var pairedDevices = DependencyService.Resolve<IBluetoothUtility>().GetPairedDevices();

            Devices = new ObservableCollection<DeviceInfo>(pairedDevices);

            OnPropertyChanged();
        }

        public void ConnectToDevice(DeviceInfo deviceInfo)
        {
            try
            {
                if (BluetoothUtility.ConnectToDeviceAndStart(deviceInfo, 250))
                {
                    ConnectedDevice = deviceInfo;
                }
            }
            catch (Exception e)
            {
                // ... could not connect to device
                Debug.WriteLine(e);
            }
        }

        public void DisconnectDevice()
        {
            BluetoothUtility.DisconnectFromDevice();

            ConnectedDevice = null;
        }

        public DeviceInfo ConnectedDevice {
            get { return connectedDevice; }
            set {
                if (connectedDevice != value)
                {
                    connectedDevice = value;
                    OnPropertyChanged(nameof(ConnectedDevice));
                    OnPropertyChanged(nameof(IsConnected));
                }
            }
        }

        public bool IsConnected => ConnectedDevice != null;
    }
}
