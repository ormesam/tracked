using System;
using System.Collections.Generic;
using System.Linq;
using MtbMate.Utilities;
using Newtonsoft.Json;
using Shared;

namespace MtbMate.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Ride : IRide {
        [JsonProperty]
        public int? RideId { get; set; }
        [JsonProperty]
        public Guid? Id { get; set; }
        [JsonProperty]
        public DateTime Start { get; set; }
        [JsonProperty]
        public DateTime End { get; set; }
        [JsonProperty]
        public IList<Location> Locations { get; set; }
        [JsonProperty]
        public IList<Jump> Jumps { get; set; }
        [JsonProperty]
        public IList<AccelerometerReading> AccelerometerReadings { get; set; }

        public string DisplayName => Start.ToString("dd/MM/yy HH:mm");

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
                Start = MovingLocations[result.StartIdx].Timestamp,
                End = MovingLocations[result.EndIdx].Timestamp,
            };

            attempt.Medal = GetMedal(attempt.Time, segment.Id.Value);

            return attempt;
        }

        private Medal GetMedal(TimeSpan time, Guid segmentId) {
            var existingAttempts = Model.Instance.SegmentAttempts
                .Where(i => i.Ride.Start < Start)
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
    }
}
