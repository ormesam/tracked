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

        public IList<SegmentAttemptModel> GetMatchingSegments(IList<SegmentModel> segments) {
            IList<LatLngModel> locationsToAnalyse = Locations
                .Where(i => i.Mph >= 1)
                .Select(i => i.LatLong)
                .ToList();

            IList<SegmentAttemptModel> matchingSegments = new List<SegmentAttemptModel>();

            foreach (var segment in segments) {
                if (locationsToAnalyse.HasPointOnLine(segment.Start) && locationsToAnalyse.HasPointOnLine(segment.End)) {
                    if (PolyUtils.LocationsMatch(segment, locationsToAnalyse)) {
                        matchingSegments.Add(new SegmentAttemptModel {
                            Created = DateTime.UtcNow,
                            RideId = Id,
                            SegmentId = segment.Id,
                        });
                    }
                }
            }

            return matchingSegments;
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
