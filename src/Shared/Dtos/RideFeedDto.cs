using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class RideFeedDto : FeedBaseDto {
        public int? RideId { get; set; }
        public DateTime EndUtc { get; set; }
        public string Name { get; set; }
        public double MaxSpeedMph { get; set; }
        public double DistanceMiles { get; set; }
        public string RouteSvgPath { get; set; }
        public IEnumerable<Medal> Medals { get; set; }

        public string FormattedTime {
            get {
                var time = EndUtc - Date;

                if (time < TimeSpan.FromMinutes(1)) {
                    return $"{time:ss}s".Trim('0');
                }

                if (time < TimeSpan.FromHours(1)) {
                    return $"{time:mm}m {time:ss}s".Trim('0'); ;
                }

                return $"{time:HH}h {time:mm}m {time:ss}s".Trim('0'); ;
            }
        }

        public RideFeedDto() {
            Medals = new List<Medal>();
        }
    }
}
