using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MtbMate.Models;
using Xamarin.Essentials;

namespace MtbMate.Accelerometer
{
    public class PhoneAccelerometerUtility : IAccelerometerUtility
    {
        #region Singleton stuff

        private static PhoneAccelerometerUtility instance;
        private static readonly object _lock = new object();

        public static PhoneAccelerometerUtility Instance {
            get {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new PhoneAccelerometerUtility();
                    }

                    return instance;
                }
            }
        }

        #endregion

        private SensorSpeed speed = SensorSpeed.Default;
        public event AccelerometerChangedEventHandler AccelerometerChanged;

        private PhoneAccelerometerUtility()
        {
            Xamarin.Essentials.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        private void Accelerometer_ReadingChanged(object sender, Xamarin.Essentials.AccelerometerChangedEventArgs e)
        {
            AccelerometerData data = e.Reading;

            var model = new AccelerometerReadingModel
            {
                Timestamp = DateTime.UtcNow,
                X = data.Acceleration.X,
                Y = data.Acceleration.Y,
                Z = data.Acceleration.Z,
            };

            AddReading(model);
        }

        public Task Start()
        {
            if (!Xamarin.Essentials.Accelerometer.IsMonitoring)
            {
                Xamarin.Essentials.Accelerometer.Start(speed);
            }

            return Task.CompletedTask;
        }

        public Task Stop()
        {
            if (Xamarin.Essentials.Accelerometer.IsMonitoring)
            {
                Xamarin.Essentials.Accelerometer.Stop();
            }

            return Task.CompletedTask;
        }

        public void AddReading(AccelerometerReadingModel reading)
        {
            AccelerometerChanged?.Invoke(new AccelerometerChangedEventArgs
            {
                Data = reading,
            });
        }
    }
}
