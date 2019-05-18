using MtbMate.Models;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace MtbMate.Utilities
{
    public class AccelerometerUtility
    {
        private SensorSpeed speed = SensorSpeed.Default;
        private Queue<AccelerometerReadingModel> readings;
        public event JumpEventHandler JumpDetected;

        public AccelerometerUtility()
        {
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
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

            readings.Enqueue(model);

            // should we do this everytime? maybe have a seperate timer which checks the last few seconds or so?
            CheckForJump(model);
        }

        private void CheckForJump(AccelerometerReadingModel model)
        {
            if (HasJumpOccured(model))
            {
                JumpDetected?.Invoke(new JumpEventArgs
                {
                    // what should we have here?
                });
            }
        }

        private bool HasJumpOccured(AccelerometerReadingModel model)
        {
            return model.Z > 5; // ??
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
