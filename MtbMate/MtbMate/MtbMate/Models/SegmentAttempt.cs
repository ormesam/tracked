using System;
using System.Collections.Generic;
using System.Linq;
using MtbMate.Utilities;
using Newtonsoft.Json;

namespace MtbMate.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SegmentAttempt {
        [JsonProperty]
        public Guid? Id { get; set; }
        [JsonProperty]
        public Guid? SegmentId { get; set; }
        [JsonProperty]
        public Guid? RideId { get; set; }
        [JsonProperty]
        public DateTime Created { get; set; }
        [JsonProperty]
        public int StartIdx { get; set; }
        [JsonProperty]
        public int EndIdx { get; set; }
        [JsonProperty]
        public Medal Medal { get; set; }
        public string DisplayName => Created.ToString("dd/MM/yy HH:mm");

        public Segment Segment => Model.Instance.Segments
            .Where(i => i.Id == SegmentId)
            .SingleOrDefault();

        public Ride Ride => Model.Instance.Rides
            .Where(i => i.Id == RideId)
            .SingleOrDefault();

        public IList<Location> Locations => Ride.MovingLocations
            .GetRange(StartIdx, (EndIdx - StartIdx) + 1);

        public IList<Jump> Jumps => Ride.Jumps
            .Where(i => i.Time >= Ride.MovingLocations[StartIdx].Timestamp)
            .Where(i => i.Time <= Ride.MovingLocations[EndIdx].Timestamp)
            .ToList();

        public TimeSpan Time => Locations.Last().Timestamp - Locations.First().Timestamp;

        public int Seconds => Time.Seconds;

        public string FormattedTime => Time.ToString(@"mm\:ss");

        public double Distance => Locations.CalculateDistanceKm();

        public double AverageSpeed => Locations.Average(i => i.Mph);

        public double MaxSpeed => Locations.Max(i => i.Mph);
    }
}
