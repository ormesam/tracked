using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Models;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace MtbMate.Accelerometer
{
    public class BleAccelerometerUtility : IAccelerometerUtility
    {
        #region Singleton stuff

        private static BleAccelerometerUtility instance;
        private static readonly object _lock = new object();

        public static BleAccelerometerUtility Instance {
            get {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new BleAccelerometerUtility();
                    }

                    return instance;
                }
            }
        }

        #endregion
        private IAdapter adapter => CrossBluetoothLE.Current.Adapter;
        private ICharacteristic characteristic;

        public event AccelerometerChangedEventHandler AccelerometerChanged;

        private BleAccelerometerUtility()
        {
            adapter.DeviceConnected += Adapter_DeviceConnected;
            adapter.DeviceDisconnected += Adapter_DeviceDisconnected;
        }

        private async void Adapter_DeviceDisconnected(object sender, DeviceEventArgs e)
        {
            await Stop();

            characteristic.ValueUpdated -= Characteristic_ValueUpdated;
            characteristic = null;
        }

        private void Characteristic_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            string value = e.Characteristic.StringValue;

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            Debug.WriteLine(DateTime.Now + " - " + value);
        }

        private void Adapter_DeviceConnected(object sender, DeviceEventArgs e)
        {
            var device = e.Device;

            device.ServicesDiscovered += async (s, ev) =>
            {
                string serviceId = "19B10001-E8F2-537E-4F6C-D104768A1214".ToLowerInvariant();

                IService service = device.Services
                    .Where(i => i.Id.ToString().ToLowerInvariant() == serviceId)
                    .SingleOrDefault();

                var characteristics = await service.GetCharacteristicsAsync();

                characteristic = characteristics.First();
                characteristic.ValueUpdated += Characteristic_ValueUpdated;
            };

            device.DiscoverServices();
        }

        public void AddReading(AccelerometerReadingModel reading)
        {
            AccelerometerChanged?.Invoke(new AccelerometerChangedEventArgs
            {
                Data = reading,
            });
        }

        public async Task Start()
        {
            await characteristic?.StartUpdatesAsync();
        }

        public async Task Stop()
        {
            await characteristic?.StopUpdatesAsync();
        }
    }
}
