using System;
using System.Collections.Generic;
using Essentials.Core.Extensions;

namespace Shared.Dtos {
    public class RideFeedDto : FeedBaseDto {
        public int? RideId { get; set; }
        public DateTime EndUtc { get; set; }
        public string Name { get; set; }
        public double MaxSpeedMph { get; set; }
        public double DistanceMiles { get; set; }
        public string RouteSvgPath { get; set; }
        public IEnumerable<Medal> Medals { get; set; }

        public string FormattedTime => (EndUtc - Date).ToReadableString();

        public RideFeedDto() {
            Medals = new List<Medal>();
        }
    }
}
