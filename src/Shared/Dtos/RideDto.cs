using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class RideDto {
        public int RideId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public string Name { get; set; }
        public decimal MaxSpeedMph { get; set; }
        public decimal AverageSpeedMph { get; set; }
        public decimal DistanceMiles { get; set; }
        public IList<RideLocationDto> Locations { get; set; }
        public IList<RideJumpDto> Jumps { get; set; }
        public IList<SegmentAttemptOverviewDto> SegmentAttempts { get; set; }
        public string DisplayName => Name ?? StartUtc.ToString("dd MMM yy HH:mm");

        public RideDto() {
            Locations = new List<RideLocationDto>();
            Jumps = new List<RideJumpDto>();
            SegmentAttempts = new List<SegmentAttemptOverviewDto>();
        }
    }
}
