using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Utilities;
using Xamarin.Forms;

namespace MtbMate.Screens.Bluetooth
{
    public class BluetoothSetupScreenViewModel : ViewModelBase
    {
        public ObservableCollection<DeviceInfo> Devices { get; }

        public BluetoothSetupScreenViewModel()
        {
            var pairedDevices = DependencyService.Resolve<IBluetoothUtility>().PairedDevices();

            Devices = new ObservableCollection<DeviceInfo>(pairedDevices);
        }

        public async Task ConnectToDevice(DeviceInfo deviceInfo)
        {
            try
            {
                DependencyService.Resolve<IBluetoothUtility>().Start(deviceInfo.Name, 250);
            }
            catch (Exception e)
            {
                // ... could not connect to device
                Debug.WriteLine(e);
            }
        }

        public async Task DisconnectDevice()
        {
            try
            {
                DependencyService.Resolve<IBluetoothUtility>().Cancel();
            }
            catch (Exception e)
            {
                // ... could not connect to device
                Debug.WriteLine(e);
            }
        }
    }
}
