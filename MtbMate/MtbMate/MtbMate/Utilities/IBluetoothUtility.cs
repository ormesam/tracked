using System.Collections.Generic;
using System.Threading.Tasks;

namespace MtbMate.Utilities
{
    public interface IBluetoothUtility
    {
        void TurnBluetoothOn();
        IList<DeviceInfo> GetPairedDevices();
        bool ConnectToDeviceAndStart(DeviceInfo deviceInfo, int sleepTime);
        void DisconnectFromDevice();
        void StartCollectingData();
        void StopCollectingData();
        DeviceInfo GetConnectedDevice();
    }
}
