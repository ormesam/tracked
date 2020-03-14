using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class RideOverviewDto {
        public Guid? ClientId { get; set; }
        public int? RideId { get; set; }
        public DateTime StartUtc { get; set; }
        public string Name { get; set; }
        public IEnumerable<Medal> Medals { get; set; }
        public bool IsAwaitingUpload { get; set; }
        public bool IsUploading { get; set; }
        public DateTime StartLocal => StartUtc.ToLocalTime();
        public string DisplayName => Name ?? StartLocal.ToString("dd MMM yy HH:mm");

        public RideOverviewDto() {
            Medals = new List<Medal>();
        }
    }
}
