using Plugin.BLE.Abstractions.Contracts;

namespace MtbMate.Utilities
{
    public class DeviceInfo
    {
        public IDevice Device { get; set; }
        public string DisplayName { get; set; }
    }
}
