using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class RideOverviewDto {
        public int? RideId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public string Name { get; set; }
        public double MaxSpeedMph { get; set; }
        public int JumpCount { get; set; }
        public double DistanceMiles { get; set; }
        public IEnumerable<Medal> Medals { get; set; }
        public DateTime StartLocal => StartUtc.ToLocalTime();
        public TimeSpan Time => EndUtc - StartUtc;
        public string Title => Name ?? StartLocal.ToString("dd MMM yy HH:mm");

        public RideOverviewDto() {
            Medals = new List<Medal>();
        }
    }
}
