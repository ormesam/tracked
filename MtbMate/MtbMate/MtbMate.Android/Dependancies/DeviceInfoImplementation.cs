using MtbMate.Droid.Dependancies;
using MtbMate.Utilities;
using Plugin.BLE.Abstractions.Contracts;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceInfoImplementation))]
namespace MtbMate.Droid.Dependancies
{
    public class DeviceInfoImplementation : IDeviceInfo
    {
        public DeviceInfo GetDeviceInfo(IDevice device)
        {
            string displayName = device.Name;

            if (string.IsNullOrWhiteSpace(displayName) &&
                device.NativeDevice is Android.Bluetooth.BluetoothDevice bluetoothDevice)
            {
                displayName = bluetoothDevice.Address;
            }

            return new DeviceInfo
            {
                DisplayName = displayName,
                Device = device,
            };
        }
    }
}