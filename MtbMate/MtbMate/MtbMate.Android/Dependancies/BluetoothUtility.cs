using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.Bluetooth;
using Java.IO;
using Java.Util;
using MtbMate.Droid.Dependancies;
using MtbMate.Models;
using MtbMate.Utilities;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothUtility))]
namespace MtbMate.Droid.Dependancies
{
    public class BluetoothUtility : IBluetoothUtility
    {
        private BluetoothAdapter adapter;
        private CancellationTokenSource cancellationToken;
        private BluetoothSocket socket;
        private BluetoothDevice connectedDevice;

        public BluetoothUtility()
        {
            adapter = BluetoothAdapter.DefaultAdapter;
        }

        public bool TurnBluetoothOn()
        {
            if (!adapter.IsEnabled)
            {
                return adapter.Enable();
            }

            return true;
        }

        public IList<DeviceInfo> GetPairedDevices()
        {
            return adapter.BondedDevices
                .Select(i => new DeviceInfo
                {
                    Name = i.Name,
                })
                .ToList();
        }

        public bool ConnectToDeviceAndStart(DeviceInfo deviceInfo, int sleepTime)
        {
            Debug.WriteLine("Trying to connect to " + deviceInfo.Name);

            connectedDevice = adapter.BondedDevices
                .Where(i => i.Name == deviceInfo.Name)
                .SingleOrDefault();

            if (connectedDevice == null)
            {
                Debug.WriteLine(deviceInfo.Name + " not found.");

                return false;
            }

            Debug.WriteLine("Connection Successful");

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            Task.Run(async () => Loop(sleepTime));
#pragma warning restore CS1998

            return true;
        }

        private async Task Loop(int sleepTime)
        {
            cancellationToken = new CancellationTokenSource();

            socket = GetSocket();

            if (socket == null)
            {
                Debug.WriteLine("Socket not found");
                return;
            }

            try
            {
                await socket.ConnectAsync();

                if (socket.IsConnected)
                {
                    Debug.WriteLine("Connected to socket");

                    var mReader = new InputStreamReader(socket.InputStream);
                    var buffer = new BufferedReader(mReader);

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (!socket.IsConnected)
                        {
                            break;
                        }

                        if (buffer.Ready())
                        {
                            string value = await buffer.ReadLineAsync();

                            ParseAndAddData(value);
                        }

                        await Task.Delay(sleepTime);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Cannot connect to socket: " + e);
            }
            finally
            {
                socket?.Close();
            }
        }

        private void ParseAndAddData(string value)
        {
            if (value.Length > 0)
            {
                double[] xyz = new double[3];
                string[] parsedData = value.Split(',');

                if (parsedData.Length != 3)
                {
                    return;
                }

                for (int i = 0; i < xyz.Length; i++)
                {
                    if (double.TryParse(parsedData[i], out double result))
                    {
                        xyz[i] = result;
                    }
                }

                var data = new AccelerometerReadingModel
                {
                    TimeStamp = DateTime.UtcNow,
                    X = xyz[0],
                    Y = xyz[1],
                    Z = xyz[2],
                };

                BluetoothAccelerometerUtility.Instance.AddReading(data);
            }
        }

        private BluetoothSocket GetSocket()
        {
            if (connectedDevice == null)
            {
                return null;
            }

            UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");

            if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4
            {
                return connectedDevice.CreateInsecureRfcommSocketToServiceRecord(uuid);
            }
            else
            {
                return connectedDevice.CreateRfcommSocketToServiceRecord(uuid);
            }
        }

        public void DisconnectFromDevice()
        {
            Debug.WriteLine("Cancellation requested");

            StopCollectingData();

            cancellationToken?.Cancel();
        }

        public void StartCollectingData()
        {
            if (socket != null && socket.IsConnected)
            {
                var buffer = Encoding.ASCII.GetBytes("r");
                socket.OutputStream.Write(buffer, 0, buffer.Length);
            }
        }

        public void StopCollectingData()
        {
            if (socket != null && socket.IsConnected)
            {
                var buffer = Encoding.ASCII.GetBytes("x");
                socket.OutputStream.Write(buffer, 0, buffer.Length);
            }
        }

        public DeviceInfo GetConnectedDevice()
        {
            if (connectedDevice == null)
            {
                return null;
            }

            return new DeviceInfo
            {
                Name = connectedDevice.Name,
            };
        }

        public bool IsBluetoothOn() => adapter.IsEnabled;
    }
}