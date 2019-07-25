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
        public IList<LocationSegmentModel> LocationSegments { get; set; }
        public IList<JumpModel> Jumps { get; set; }
        public IList<AccelerometerReadingModel> AccelerometerReadings { get; set; }
        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? Start?.ToString("dd/MM/yy HH:mm") : Name;

        //public IAccelerometerUtility AccelerometerUtility => PhoneAccelerometerUtility.Instance;
        public IAccelerometerUtility AccelerometerUtility => BleAccelerometerUtility.Instance;

        public RideModel()
        {
            LocationSegments = new List<LocationSegmentModel>();
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

            GeoUtility.Instance.Start();
            await AccelerometerUtility.Start();
        }

        public async Task StopRide()
        {
            End = DateTime.UtcNow;

            GeoUtility.Instance.Stop();
            await AccelerometerUtility.Stop();

            AccelerometerUtility.AccelerometerChanged -= AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged -= GeoUtility_LocationChanged;

            foreach (var segment in LocationSegments)
            {
                segment.CalculateValues();
            }

            // CheckForJumpsAndDrops();
        }

        private void AccelerometerUtility_AccelerometerChanged(Accelerometer.AccelerometerChangedEventArgs e)
        {
            AccelerometerReadings.Add(e.Data);
            Debug.WriteLine(e.Data);
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e)
        {
            LocationSegments.Add(e.Location);
            Debug.WriteLine(e.Location);
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
                    .Where(i => i.Timestamp >= drop.Timestamp - jumpAllowance)
                    .Where(i => i.Timestamp <= drop.Timestamp);

                var readingsAfterDrop = AccelerometerReadings
                    .Where(i => i.Timestamp <= drop.Timestamp + jumpAllowance)
                    .Where(i => i.Timestamp >= drop.Timestamp);

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
                    TakeOffTimeStamp = takeOffReading.Timestamp,
                    LandingGForce = landingReading.Z,
                    LandingTimeStamp = landingReading.Timestamp,
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
                sb.AppendLine($"{reading.Timestamp},{reading.X},{reading.Y},{reading.Z}");
            }

            sb.AppendLine();

            string fileName = "Ride Data.txt";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllText(filePath, sb.ToString());

            return new ShareFile(filePath);
        }

        public ShareFile GetLocationFile()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("TimeStamp,Lat,Lon,Timestamp,Lat,Lon,Distance,Speed");

            foreach (var segment in LocationSegments)
            {
                sb.AppendLine($"{segment.Start.Timestamp},{segment.Start.Latitude},{segment.Start.Longitude}" +
                    $",{segment.End.Timestamp},{segment.End.Latitude},{segment.End.Longitude}" +
                    $",{segment.Distance},{segment.Mph}");
            }

            sb.AppendLine();

            string fileName = "Ride Data.txt";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllText(filePath, sb.ToString());

            return new ShareFile(filePath);
        }
    }
}
