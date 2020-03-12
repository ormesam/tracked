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
        public string DisplayName => Name ?? StartUtc.ToString("dd MMM yy HH:mm");
        public decimal MaxSpeedMph => Locations.Max(i => i.Mph);
        public decimal AverageSpeedMph => Locations.Average(i => i.Mph);
        public decimal DistanceMiles => DistanceHelpers.GetDistanceMile(Locations.Cast<ILatLng>().ToList());
        public TimeSpan Time => EndUtc - StartUtc;

        public RideDto() {
            Locations = new List<RideLocationDto>();
            Jumps = new List<JumpDto>();
            SegmentAttempts = new List<SegmentAttemptOverviewDto>();
        }
    }
}
