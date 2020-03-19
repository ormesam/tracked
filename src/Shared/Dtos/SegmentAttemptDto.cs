using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Interfaces;

namespace Shared.Dtos {
    public class SegmentAttemptDto {
        public int SegmentAttemptId { get; set; }
        public int SegmentId { get; set; }
        public int RideId { get; set; }
        public Medal Medal { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public IList<RideLocationDto> Locations { get; set; }
        public IList<JumpDto> Jumps { get; set; }
        public TimeSpan Time => EndUtc - StartUtc;
        public string FormattedTime => Time.ToString(@"mm\:ss");
        public int JumpCount => Jumps.Count;
        public string MaxAirtime => Jumps.Count == 0 ? "-" : $"{Jumps.Max(i => i.Airtime)}s";
        public decimal MaxSpeedMph => Locations.Max(i => i.Mph);
        public decimal AverageSpeedMph => Locations.Average(i => i.Mph);
        public decimal DistanceMiles => DistanceHelpers.GetDistanceMile(Locations.Cast<ILatLng>().ToList());

        public SegmentAttemptDto() {
            Locations = new List<RideLocationDto>();
            Jumps = new List<JumpDto>();
        }
    }
}
