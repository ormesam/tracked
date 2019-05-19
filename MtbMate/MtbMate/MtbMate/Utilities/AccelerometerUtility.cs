using MtbMate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace MtbMate.Utilities
{
    public class AccelerometerUtility
    {
        private SensorSpeed speed = SensorSpeed.Default;
        private readonly Queue<AccelerometerReadingModel> readings;
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
                //readings.Dequeue();
            }

            readings.Enqueue(model);
        }

        public void Start()
        {
            if (!Accelerometer.IsMonitoring)
            {
                readings.Clear();

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

        public void CheckForEvents()
        {
            CheckForJumpsAndDrops();
        }

        private void CheckForJumpsAndDrops()
        {
            bool previousReadingBelowLowerLimit = false;
            double landingUpperLimit = 3;
            double takeoffUpperLimit = 2;
            double lowerLimit = -1;
            // temp or now
            TimeSpan jumpAllowance = TimeSpan.FromSeconds(1.5);

            var dropReadings = new List<AccelerometerReadingModel>();

            foreach (var reading in readings)
            {
                // The z index is lower than the lower limit when at the top of the jump.
                if (reading.Z > lowerLimit)
                {
                    previousReadingBelowLowerLimit = false;
                    continue;
                }

                if (previousReadingBelowLowerLimit)
                {
                    continue;
                }

                previousReadingBelowLowerLimit = true;

                dropReadings.Add(reading);
            }

            foreach (var drop in dropReadings)
            {
                var readingsBeforeDrop = readings
                    .Where(i => i.TimeStamp >= drop.TimeStamp - jumpAllowance)
                    .Where(i => i.TimeStamp <= drop.TimeStamp);

                var readingsAfterDrop = readings
                    .Where(i => i.TimeStamp <= drop.TimeStamp + jumpAllowance)
                    .Where(i => i.TimeStamp >= drop.TimeStamp);

                var takeOffReading = readingsBeforeDrop
                    .Where(i => i.Z > takeoffUpperLimit)
                    .Where(i => i.Z == readingsBeforeDrop.Max(j => j.Z))
                    .FirstOrDefault();

                var landingReading = readingsAfterDrop
                    .Where(i => i.Z > landingUpperLimit)
                    .Where(i => i.Z == readingsAfterDrop.Max(j => j.Z))
                    .FirstOrDefault();

                if (takeOffReading == null || landingReading == null)
                {
                    continue;
                }

                JumpModel jump = new JumpModel
                {
                    TakeOffGForce = takeOffReading.Z,
                    TakeOffTimeStamp = takeOffReading.TimeStamp,
                    LandingGForce = landingReading.Z,
                    LandingTimeStamp = landingReading.TimeStamp,
                };

                JumpDetected?.Invoke(new JumpEventArgs
                {
                    Jump = jump,
                });
            }
        }

        public string GetReadings()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"TimeStamp,X,Y,Z");

            foreach (var reading in readings)
            {
                sb.AppendLine($"{reading.TimeStamp.ToString("hh:mm:ss.fff")},{reading.X},{reading.Y},{reading.Z}");
            }

            return sb.ToString();
        }
    }
}
