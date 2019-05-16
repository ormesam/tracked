using MtbMate.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace MtbMate.Utilities
{
    public class AccelerometerUtility
    {
        private SensorSpeed speed = SensorSpeed.Default;
        private readonly IDisplay display;
        private Queue<AccelerometerReadingModel> readings;

        public AccelerometerUtility(IDisplay display)
        {
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            this.display = display;
            readings = new Queue<AccelerometerReadingModel>();
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            AccelerometerData data = e.Reading;

            var model = new AccelerometerReadingModel
            {
                TimeStamp = DateTime.UtcNow,
                X = data.Acceleration.X,
                Y = data.Acceleration.Y,
                Z = data.Acceleration.Z,
            };

            Console.WriteLine(model.ToString());

            if (readings.Count > 1000)
            {
                readings.Dequeue();
            }

            if (model.Z > 80)
            {
                display.ShowJump(model);
            }

            readings.Enqueue(model);
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
