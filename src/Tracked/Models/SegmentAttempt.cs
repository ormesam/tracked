using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Shared;

namespace Tracked.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SegmentAttempt : IRide {
        [JsonProperty]
        public int? SegmentAttemptId { get; set; }
        [JsonProperty]
        public Guid? Id { get; set; }
        [JsonProperty]
        public Guid? SegmentId { get; set; }
        [JsonProperty]
        public Guid? RideId { get; set; }
        [JsonProperty]
        public DateTime Start { get; set; }
        [JsonProperty]
        public DateTime End { get; set; }
        [JsonProperty]
        public Medal Medal { get; set; }

        public string DisplayName => Start.ToString("dd/MM/yy HH:mm");

        public Segment Segment => Model.Instance.Segments
            .Where(i => i.Id == SegmentId)
            .SingleOrDefault();

        public Ride Ride => Model.Instance.Rides
            .Where(i => i.Id == RideId)
            .SingleOrDefault();

        public IList<Location> Locations => Ride.MovingLocations
            .Where(i => i.Timestamp >= Start)
            .Where(i => i.Timestamp <= End)
            .ToList();

        public IList<AccelerometerReading> AccelerometerReadings => Ride.AccelerometerReadings
            .Where(i => i.Timestamp >= Start)
            .Where(i => i.Timestamp <= End)
            .ToList();

        public IList<Jump> Jumps => Ride.Jumps
            .Where(i => i.Timestamp >= Start)
            .Where(i => i.Timestamp <= End)
            .ToList();

        public TimeSpan Time => Locations.Last().Timestamp - Locations.First().Timestamp;

        public int Seconds => Time.Seconds;

        public string FormattedTime => Time.ToString(@"mm\:ss");

        public double Distance => 0; // Locations.CalculateDistanceKm();

        public double AverageSpeed => Locations.Average(i => i.Mph);

        public double MaxSpeed => Locations.Max(i => i.Mph);

        public bool ShowAttempts => false;
    }
}
