using System.Collections.Generic;

namespace MtbMate.Utilities
{
    public interface IBluetoothUtility
    {
        void Start(string name, int sleepTime);
        void Cancel();
        IList<DeviceInfo> PairedDevices();
    }
}
