using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class RideOverviewDto {
        public int RideId { get; set; }
        public DateTime StartUtc { get; set; }
        public string Name { get; set; }
        public IEnumerable<Medal> Medals { get; set; }
        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? StartUtc.ToString("dd MMM yy HH:mm") : Name;
        public DateTime StartLocal => StartUtc.ToLocalTime();
    }
}
