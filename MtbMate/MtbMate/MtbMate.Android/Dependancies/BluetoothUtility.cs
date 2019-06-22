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

        public BluetoothUtility()
        {
            adapter = BluetoothAdapter.DefaultAdapter;
        }

        public void Cancel()
        {
            Debug.WriteLine("Cancellation requested");

            cancellationToken?.Cancel();
        }

        public IList<DeviceInfo> PairedDevices()
        {
            return adapter.BondedDevices
                .Select(i => new DeviceInfo
                {
                    Name = i.Name,
                    Status = GetStatus(i.BondState),
                })
                .ToList();
        }

        private BluetoothConnectionStatus GetStatus(Bond bondState)
        {
            switch (bondState)
            {
                case Bond.Bonded:
                    return BluetoothConnectionStatus.Connected;
                case Bond.Bonding:
                    return BluetoothConnectionStatus.Connecting;
                default:
                    return BluetoothConnectionStatus.None;
            }
        }

        public void Start(string name, int sleepTime)
        {
            if (!adapter.IsEnabled)
            {
                return;
            }

            Task.Run(async () => Loop(name, sleepTime));
        }

        private async Task Loop(string name, int sleepTime)
        {
            cancellationToken = new CancellationTokenSource();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(sleepTime);

                    if (adapter == null)
                    {
                        Debug.WriteLine("No Bluetooth adapter found.");
                        continue;
                    }

                    if (!adapter.IsEnabled)
                    {
                        Debug.WriteLine("Bluetooth adapter is not enabled.");
                        continue;
                    }

                    Debug.WriteLine("Trying to connect to " + name);

                    BluetoothDevice device = adapter.BondedDevices
                        .Where(i => i.Name == name)
                        .SingleOrDefault();

                    if (device == null)
                    {
                        Debug.WriteLine(name + " not found.");
                        continue;
                    }

                    Debug.WriteLine("Connection Successful");

                    UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");

                    if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4
                    {
                        socket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);
                    }
                    else
                    {
                        socket = device.CreateRfcommSocketToServiceRecord(uuid);
                    }

                    if (socket == null)
                    {
                        Debug.WriteLine("Socket not found");
                        continue; // should we return?
                    }

                    await socket.ConnectAsync();

                    if (socket.IsConnected)
                    {
                        Debug.WriteLine("Connected!");

                        var mReader = new InputStreamReader(socket.InputStream);
                        var buffer = new BufferedReader(mReader);

                        while (!cancellationToken.IsCancellationRequested)
                        {
                            if (buffer.Ready())
                            {
                                string value = await buffer.ReadLineAsync();

                                if (value.Length > 0)
                                {
                                    double[] xyz = new double[3];
                                    string[] parsedData = value.Split(',');

                                    if (parsedData.Length != 3)
                                    {
                                        continue;
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

                                    AccelerometerUtility.Instance.AddReading(data);
                                }
                            }

                            // A little stop to the uneverending thread...
                            await Task.Delay(sleepTime);

                            if (!socket.IsConnected)
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("EXCEPTION: " + ex.Message);
                }
                finally
                {
                    socket?.Close();
                }
            }
        }

        public void Run()
        {
            var buffer = Encoding.ASCII.GetBytes("r");

            socket.OutputStream.Write(buffer, 0, buffer.Length);
        }

        public void Stop()
        {
            var buffer = Encoding.ASCII.GetBytes("x");

            socket.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }
}