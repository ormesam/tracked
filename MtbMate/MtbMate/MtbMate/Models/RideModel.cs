using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MtbMate.Accelerometer;
using MtbMate.Utilities;
using Xamarin.Essentials;

namespace MtbMate.Models
{
    public class RideModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public IList<LocationModel> Locations { get; set; }
        public IList<JumpModel> Jumps { get; set; }
        public IList<AccelerometerReadingModel> AccelerometerReadings { get; set; }
        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? Start?.ToString("dd/MM/yy HH:mm") : Name;

        public IAccelerometerUtility AccelerometerUtility => PhoneAccelerometerUtility.Instance; // BluetoothAccelerometerUtility.Instance;

        public RideModel()
        {
            Locations = new List<LocationModel>();
            Jumps = new List<JumpModel>();
            AccelerometerReadings = new List<AccelerometerReadingModel>();
        }

        public async Task StartRide()
        {
            if (Start == null)
            {
                Start = DateTime.UtcNow;
            }

            AccelerometerUtility.AccelerometerChanged += AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged += GeoUtility_LocationChanged;

            await GeoUtility.Instance.Start();
            AccelerometerUtility.Start();
        }

        public async Task StopRide()
        {
            End = DateTime.UtcNow;

            await GeoUtility.Instance.Stop();
            AccelerometerUtility.Stop();

            AccelerometerUtility.AccelerometerChanged -= AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged -= GeoUtility_LocationChanged;

            CheckForJumpsAndDrops();
        }

        private void AccelerometerUtility_AccelerometerChanged(Accelerometer.AccelerometerChangedEventArgs e)
        {
            AccelerometerReadings.Add(e.Data);
            Debug.WriteLine(e.Data);
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e)
        {
            Locations.Add(e.Location);
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

            foreach (var reading in AccelerometerReadings)
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
                var readingsBeforeDrop = AccelerometerReadings
                    .Where(i => i.TimeStamp >= drop.TimeStamp - jumpAllowance)
                    .Where(i => i.TimeStamp <= drop.TimeStamp);

                var readingsAfterDrop = AccelerometerReadings
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

                Jumps.Add(jump);
            }
        }

        public ShareFile GetReadingsFile()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("TimeStamp,X,Y,Z");

            foreach (var reading in AccelerometerReadings)
            {
                sb.AppendLine($"{reading.TimeStamp},{reading.X},{reading.Y},{reading.Z}");
            }

            sb.AppendLine();

            sb.AppendLine("TimeStamp,Lat,Lon,Mph");

            foreach (var location in Locations)
            {
                sb.AppendLine($"{location.Timestamp},{location.Latitude},{location.Longitude},{location.Mph}");
            }

            string fileName = "Ride Data.txt";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllText(filePath, sb.ToString());

            return new ShareFile(filePath);
        }
    }
}
