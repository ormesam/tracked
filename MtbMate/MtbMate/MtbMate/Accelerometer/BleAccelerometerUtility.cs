using MtbMate.Models;
using Xamarin.Forms;

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

        public event AccelerometerChangedEventHandler AccelerometerChanged;

        private BleAccelerometerUtility()
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
        }

        public void Stop()
        {
        }
    }
}
