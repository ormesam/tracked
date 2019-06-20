using MtbMate.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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

        private readonly ConcurrentQueue<AccelerometerReadingModel> readings;
        public event JumpEventHandler JumpDetected;

        private AccelerometerUtility()
        {
            readings = new ConcurrentQueue<AccelerometerReadingModel>();
        }

        public void AddReading(AccelerometerReadingModel reading)
        {
            readings.Enqueue(reading);

            Debug.WriteLine(reading);
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
            // temp for now
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
