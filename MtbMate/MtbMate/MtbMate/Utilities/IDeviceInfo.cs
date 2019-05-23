using Plugin.BLE.Abstractions.Contracts;

namespace MtbMate.Utilities
{
    public interface IDeviceInfo
    {
        DeviceInfo GetDeviceInfo(IDevice device);
    }
}
