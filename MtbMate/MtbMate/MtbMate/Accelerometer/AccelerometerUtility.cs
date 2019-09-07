using System;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Models;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace MtbMate.Accelerometer {
    public class AccelerometerUtility {
        #region Singleton stuff

        private static AccelerometerUtility instance;
        private static readonly object _lock = new object();

        public static AccelerometerUtility Instance {
            get {
                lock (_lock) {
                    if (instance == null) {
                        instance = new AccelerometerUtility();
                    }

                    return instance;
                }
            }
        }

        #endregion

        private IAdapter adapter => CrossBluetoothLE.Current.Adapter;

        private ICharacteristic characteristic;
        private AccelerometerStatus status;

        public event AccelerometerChangedEventHandler AccelerometerChanged;
        public event AccelerometerStatusChangedEventHandler StatusChanged;

        private AccelerometerUtility() {
            adapter.DeviceConnected += Adapter_DeviceConnected;
            adapter.DeviceDisconnected += Adapter_DeviceDisconnected;
        }

        private void Characteristic_ValueUpdated(object sender, CharacteristicUpdatedEventArgs e) {
            string value = e.Characteristic.StringValue;

            ParseAndAddData(value);
        }

        public AccelerometerStatus Status {
            get { return status; }
            set {
                if (status != value) {
                    status = value;

                    StatusChanged?.Invoke(new AccelerometerStatusChangedEventArgs {
                        NewStatus = value,
                    });
                }
            }
        }


        private void ParseAndAddData(string value) {
            if (!string.IsNullOrWhiteSpace(value)) {
                DateTime timeStamp = DateTime.UtcNow;

                double[] xyz = new double[3];
                string[] parsedData = value.Split(',');

                if (parsedData.Length != 3) {
                    return;
                }

                for (int i = 0; i < xyz.Length; i++) {
                    if (double.TryParse(parsedData[i], out double result)) {
                        xyz[i] = result;
                    }
                }

                var data = new AccelerometerReadingModel {
                    Timestamp = timeStamp,
                    X = xyz[0],
                    Y = xyz[1],
                    Z = xyz[2],
                };

                AccelerometerChanged?.Invoke(new AccelerometerChangedEventArgs {
                    Data = data,
                });
            }
        }

        private void Adapter_DeviceConnected(object sender, DeviceEventArgs e) {
            var device = e.Device;

            device.ServicesDiscovered += async (s, ev) => {
                string serviceId = "19B10001-E8F2-537E-4F6C-D104768A1214".ToLowerInvariant();

                IService service = device.Services
                    .Where(i => i.Id.ToString().ToLowerInvariant() == serviceId)
                    .SingleOrDefault();

                var characteristics = await service.GetCharacteristicsAsync();

                characteristic = characteristics.First();
                characteristic.ValueUpdated += Characteristic_ValueUpdated;

                Status = AccelerometerStatus.Ready;
            };

            Status = AccelerometerStatus.NotReady;

            device.DiscoverServices();
        }

        private void Adapter_DeviceDisconnected(object sender, DeviceEventArgs e) {
            Status = AccelerometerStatus.NotConnected;
        }

        public void AddReading(AccelerometerReadingModel reading) {
            AccelerometerChanged?.Invoke(new AccelerometerChangedEventArgs {
                Data = reading,
            });
        }

        public async Task Start() {
            if (characteristic == null) {
                return;
            }

            await characteristic.StartUpdatesAsync();
        }

        public async Task Stop() {
            if (characteristic == null) {
                return;
            }

            try {
                await characteristic.StopUpdatesAsync();
            } catch (Exception) {
            }
        }

        public async Task Reset() {
            await Stop();

            if (characteristic != null) {
                Status = AccelerometerStatus.NotReady;
                characteristic.ValueUpdated -= Characteristic_ValueUpdated;
                characteristic = null;
            }
        }
    }
}
