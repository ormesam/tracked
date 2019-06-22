using MtbMate.Models;
using System.Diagnostics;
using Xamarin.Forms;

namespace MtbMate.Utilities
{
    public class AccelerometerUtility
    {
        #region Singleton stuff

        private static AccelerometerUtility instance;
        private static readonly object _lock = new object();

        public static AccelerometerUtility Instance {
            get {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new AccelerometerUtility();
                    }

                    return instance;
                }
            }
        }

        #endregion

        public event AccelerometerChangedEventHandler AccelerometerChanged;

        private AccelerometerUtility()
        {
        }

        public void AddReading(AccelerometerReadingModel reading)
        {
            AccelerometerChanged?.Invoke(new AccelerometerChangedEventArgs
            {
                Data = reading,
            });

            Debug.WriteLine(reading);
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
