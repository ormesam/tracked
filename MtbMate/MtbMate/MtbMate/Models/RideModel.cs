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

        //public IAccelerometerUtility AccelerometerUtility => PhoneAccelerometerUtility.Instance;
        public IAccelerometerUtility AccelerometerUtility => BleAccelerometerUtility.Instance;

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

            sb.AppendLine("TimeStamp,Lat,Lon,Position Accuracy,Mps,Mps Accuracy (m),Mph,Altitude");

            foreach (var location in Locations)
            {
                sb.AppendLine($"{location.Timestamp},{location.Latitude},{location.Longitude},{location.AccuracyInMetres}," +
                    $"{location.SpeedMetresPerSecond},{location.SpeedAccuracyMetresPerSecond},{location.Mph}," +
                    $"{location.Altitude}");
            }

            sb.AppendLine();

            string fileName = "Ride Data.txt";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllText(filePath, sb.ToString());

            return new ShareFile(filePath);
        }

        public IList<LocationSegmentModel> GetLocationSegments()
        {
            var segments = new List<LocationSegmentModel>();

            for (int i = 1; i < Locations.Count; i++)
            {
                var segment = new LocationSegmentModel
                {
                    Start = Locations[i - 1],
                    End = Locations[i],
                };

                segment.CalculateValues();

                segments.Add(segment);
            }

            return segments;
        }
    }
}
