using System;
using System.Collections.Generic;
using System.Linq;
using MtbMate.Utilities;

namespace MtbMate.Models {
    public class SegmentAttempt {
        public Guid? Id { get; set; }
        public Guid? SegmentId { get; set; }
        public Guid? RideId { get; set; }
        public DateTime Created { get; set; }
        public int StartIdx { get; set; }
        public int EndIdx { get; set; }
        public string DisplayName => Created.ToString("dd/MM/yy HH:mm");

        public Segment Segment => Model.Instance.Segments
            .Where(i => i.Id == SegmentId)
            .SingleOrDefault();

        public Ride Ride => Model.Instance.Rides
            .Where(i => i.Id == RideId)
            .SingleOrDefault();

        public IList<Location> Locations => Ride.MovingLocations
            .ToList()
            .GetRange(StartIdx, (EndIdx - StartIdx) + 1);

        public string Time => (Locations.Last().Timestamp - Locations.First().Timestamp).ToString(@"mm\:ss");

        public double Distance => Locations.CalculateDistanceKm();

        public double AverageSpeed => Locations.Average(i => i.Mph);

        public double MaxSpeed => Locations.Max(i => i.Mph);
    }
}
