using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class RideDto {
        public int RideId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public string Name { get; set; }
        public double MaxSpeedMph { get; set; }
        public double AverageSpeedMph { get; set; }
        public double DistanceMiles { get; set; }
        public IList<RideLocationDto> Locations { get; set; }
        public IList<JumpDto> Jumps { get; set; }
        public IList<SegmentAttemptOverviewDto> SegmentAttempts { get; set; }
        public DateTime StartLocal => StartUtc.ToLocalTime();
        public string DisplayName => Name ?? StartLocal.ToString("dd MMM yy HH:mm");
        public TimeSpan Time => EndUtc - StartUtc;

        public RideDto() {
            Locations = new List<RideLocationDto>();
            Jumps = new List<JumpDto>();
            SegmentAttempts = new List<SegmentAttemptOverviewDto>();
        }
    }
}
