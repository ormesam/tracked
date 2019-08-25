using MtbMate.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

        public RideModel() {
            Locations = new List<LocationModel>();
            Jumps = new List<JumpModel>();
            AccelerometerReadings = new List<AccelerometerReadingModel>();
        }

        public bool MatchesSegment(SegmentModel segment) {
            var latLongs = Locations
                .Select(i => i.LatLong)
                .ToList();

            bool matchesStart = latLongs
                .HasPointOnLine(segment.Start);

            bool matchesEnd = latLongs
                .HasPointOnLine(segment.End);

            if (!matchesStart || !matchesEnd) {
                return false;
            };

            var closestPointToSegmentStart = segment.GetClosestStartPoint(Locations);
            var closestPointToSegmentEnd = segment.GetClosestEndPoint(Locations);

            if (closestPointToSegmentStart == null || closestPointToSegmentEnd == null) {
                return false;
            }

            var segmentLocations = Locations
                .Where(i => i.SpeedMetresPerSecond > 0)
                .Where(i => i.Timestamp >= closestPointToSegmentStart.Timestamp)
                .Where(i => i.Timestamp <= closestPointToSegmentEnd.Timestamp)
                .ToList();

            int matchedPointCount = 0;
            int missedPointCount = 0;

            foreach (var segmentLocation in segmentLocations) {
                if (segment.Points.HasPointOnLine(segmentLocation.LatLong)) {
                    matchedPointCount++;
                } else {
                    missedPointCount++;
                }
            }

            return matchedPointCount >= segment.Points.Count * 0.9;
        }

        public ShareFile GetReadingsFile() {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("TimeStamp,X,Y,Z");

            foreach (var reading in AccelerometerReadings) {
                sb.AppendLine($"{reading.Timestamp},{reading.X},{reading.Y},{reading.Z}");
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

            foreach (var location in Locations) {
                sb.AppendLine($"{location.Timestamp},{location.LatLong.Latitude},{location.LatLong.Longitude},{location.AccuracyInMetres}," +
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
