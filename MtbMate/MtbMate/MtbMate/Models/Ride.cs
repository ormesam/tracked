using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MtbMate.Utilities;
using Xamarin.Essentials;

namespace MtbMate.Models {
    public class Ride {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public IList<Location> Locations { get; set; }
        public IList<Jump> Jumps { get; set; }
        public IList<AccelerometerReading> AccelerometerReadings { get; set; }
        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? Start?.ToString("dd/MM/yy HH:mm") : Name;
        public IList<Location> MovingLocations => Locations
            .Where(i => i.Mph >= 1)
            .ToList();

        public Ride() {
            Locations = new List<Location>();
            Jumps = new List<Jump>();
            AccelerometerReadings = new List<AccelerometerReading>();
        }

        public SegmentAttempt MatchesSegment(Segment segment) {
            List<LatLng> locationLatLngs = MovingLocations
                .Select(i => i.LatLong)
                .ToList();

            var result = PolyUtils.LocationsMatch(segment, locationLatLngs);

            if (!result.MatchesSegment) {
                return null;
            }

            return new SegmentAttempt {
                Created = MovingLocations.First().Timestamp,
                RideId = Id,
                SegmentId = segment.Id,
                StartIdx = result.StartIdx,
                EndIdx = result.EndIdx,
            };
        }

        public ShareFile GetReadingsFile() {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("TimeStamp,Value");

            foreach (var reading in AccelerometerReadings.OrderBy(i => i.Timestamp)) {
                sb.AppendLine($"{reading.Timestamp.ToString("dd/MM/yyyy HH:mm:ss.fff")},{reading.Value}");
            }

            sb.AppendLine();

            string fileName = "Ride Data.txt";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllText(filePath, sb.ToString());

            return new ShareFile(filePath);
        }

        public ShareFile GetLocationFile() {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("TimeStamp,Lat,Lon,Position Accuracy,Mps,Mps Accuracy (m),Mph,Altitude");

            foreach (var location in Locations.OrderBy(i => i.Timestamp)) {
                sb.AppendLine($"{location.Timestamp.ToString("dd/MM/yyyy HH:mm:ss.fff")},{location.LatLong.Latitude},{location.LatLong.Longitude},{location.AccuracyInMetres}," +
                    $"{location.SpeedMetresPerSecond},{location.SpeedAccuracyMetresPerSecond},{location.Mph}," +
                    $"{location.Altitude}");
            }

            sb.AppendLine();

            string fileName = "Ride Data.txt";
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllText(filePath, sb.ToString());

            return new ShareFile(filePath);
        }
    }
}
