using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Interfaces;

namespace Shared.Dtos {
    public class RideDto {
        public int RideId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public string Name { get; set; }
        public IList<RideLocationDto> Locations { get; set; }
        public IList<JumpDto> Jumps { get; set; }
        public IList<SegmentAttemptOverviewDto> SegmentAttempts { get; set; }
        public DateTime StartLocal => StartUtc.ToLocalTime();
        public string DisplayName => Name ?? StartLocal.ToString("dd MMM yy HH:mm");
        public double MaxSpeedMph => Locations.Max(i => i.Mph);
        public double AverageSpeedMph => Locations.Average(i => i.Mph);
        public double DistanceMiles => DistanceHelpers.GetDistanceMile(Locations.Cast<ILatLng>().ToList());
        public TimeSpan Time => EndUtc - StartUtc;

        public RideDto() {
            Locations = new List<RideLocationDto>();
            Jumps = new List<JumpDto>();
            SegmentAttempts = new List<SegmentAttemptOverviewDto>();
        }
    }
}
