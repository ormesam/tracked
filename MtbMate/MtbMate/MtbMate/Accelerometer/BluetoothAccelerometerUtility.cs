using MtbMate.Models;
using MtbMate.Utilities;
using System.Diagnostics;
using Xamarin.Forms;

namespace MtbMate.Accelerometer
{
    public class BluetoothAccelerometerUtility : IAccelerometerUtility
    {
        #region Singleton stuff

        private static BluetoothAccelerometerUtility instance;
        private static readonly object _lock = new object();

        public static BluetoothAccelerometerUtility Instance {
            get {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new BluetoothAccelerometerUtility();
                    }

                    return instance;
                }
            }
        }

        #endregion

        public event AccelerometerChangedEventHandler AccelerometerChanged;

        private BluetoothAccelerometerUtility()
        {
        }

        public void AddReading(AccelerometerReadingModel reading)
        {
            AccelerometerChanged?.Invoke(new AccelerometerChangedEventArgs
            {
                Data = reading,
            });
        }

        public void Start()
        {
            DependencyService.Resolve<IBluetoothUtility>().StartCollectingData();
        }

        public void Stop()
        {
            DependencyService.Resolve<IBluetoothUtility>().StopCollectingData();
        }
    }
}
