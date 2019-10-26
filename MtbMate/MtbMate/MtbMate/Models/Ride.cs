using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MtbMate.Contexts;
using MtbMate.Utilities;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace MtbMate.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Ride : IRide {
        [JsonProperty]
        public Guid? Id { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public DateTime? Start { get; set; }
        [JsonProperty]
        public DateTime? End { get; set; }
        [JsonProperty]
        public IList<Location> Locations { get; set; }
        [JsonProperty]
        public IList<Jump> Jumps { get; set; }
        [JsonProperty]
        public IList<AccelerometerReading> AccelerometerReadings { get; set; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? Start?.ToString("dd/MM/yy HH:mm") : Name;

        public bool CanChangeName => true;

        public bool ShowAttempts => true;

        public IList<Location> MovingLocations => Locations
            .Where(i => i.Mph >= 1)
            .ToList();

        public IList<Medal> Medals => Model.Instance.SegmentAttempts
            .Where(i => i.RideId == Id)
            .Select(i => i.Medal)
            .ToList();

        public Ride() {
            Locations = new List<Location>();
            Jumps = new List<Jump>();
            AccelerometerReadings = new List<AccelerometerReading>();
        }

        public SegmentAttempt MatchesSegment(Segment segment) {
            List<LatLng> locationLatLngs = MovingLocations
                .Select(i => i.Point)
                .ToList();

            var result = PolyUtils.LocationsMatch(segment, locationLatLngs);

            if (!result.MatchesSegment) {
                return null;
            }

            SegmentAttempt attempt = new SegmentAttempt {
                Created = MovingLocations.First().Timestamp,
                RideId = Id,
                SegmentId = segment.Id,
                StartIdx = result.StartIdx,
                EndIdx = result.EndIdx,
            };

            attempt.Medal = GetMedal(attempt.Time, segment.Id.Value);

            return attempt;
        }

        private Medal GetMedal(TimeSpan time, Guid segmentId) {
            var existingAttempts = Model.Instance.SegmentAttempts
                .Where(i => i.SegmentId == segmentId)
                .OrderBy(i => i.Time)
                .Select(i => i.Time)
                .ToList();

            if (!existingAttempts.Any()) {
                return Medal.None;
            }

            if (existingAttempts.Count == 1) {
                return time < existingAttempts[0] ? Medal.Gold : Medal.Silver;
            }

            if (existingAttempts.Count == 2) {
                return time < existingAttempts[0] ? Medal.Gold : time < existingAttempts[1] ? Medal.Silver : Medal.Bronze;
            }

            if (time < existingAttempts.FirstOrDefault()) {
                return Medal.Gold;
            } else if (time < existingAttempts.Skip(1).FirstOrDefault()) {
                return Medal.Silver;
            } else if (time < existingAttempts.Skip(2).FirstOrDefault()) {
                return Medal.Bronze;
            }

            return Medal.None;
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

        public void ChangeName(UIContext ui, Action whenComplete) {
            ui.ShowInputDialog("Change Name", Name, async (newName) => {
                Name = newName;

                await Model.Instance.SaveRide(this);

                whenComplete?.Invoke();
            });
        }
    }
}
