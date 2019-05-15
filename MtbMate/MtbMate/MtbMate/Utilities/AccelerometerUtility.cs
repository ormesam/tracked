using System;
using Xamarin.Essentials;

namespace MtbMate.Utilities
{
    public class AccelerometerUtility
    {
        private SensorSpeed speed = SensorSpeed.Default;

        public AccelerometerUtility()
        {
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            AccelerometerData data = e.Reading;

            Console.WriteLine($"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");
        }

        public void Start()
        {
            if (!Accelerometer.IsMonitoring)
            {
                Accelerometer.Start(speed);
            }
        }

        public void Stop()
        {
            if (Accelerometer.IsMonitoring)
            {
                Accelerometer.Stop();
            }
        }

        public void ChangeSpeed(SensorSpeed speed)
        {
            Stop();
            this.speed = speed;
            Start();
        }
    }
}
