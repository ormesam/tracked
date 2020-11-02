using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class RideDto {
        public int? RideId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public string Name { get; set; }
        public double MaxSpeedMph { get; set; }
        public double AverageSpeedMph { get; set; }
        public double DistanceMiles { get; set; }
        public IList<RideLocationDto> Locations { get; set; }
        public IList<JumpDto> Jumps { get; set; }
        public IList<SegmentAttemptOverviewDto> SegmentAttempts { get; set; }

        public string FormattedTime {
            get {
                var time = EndUtc.ToLocalTime() - StartUtc.ToLocalTime();

                if (time < TimeSpan.FromMinutes(1)) {
                    return $"{time:ss}s".Trim('0');
                }

                if (time < TimeSpan.FromHours(1)) {
                    return $"{time:mm}m {time:ss}s".Trim('0'); ;
                }

                return $"{time:HH}h {time:mm}m {time:ss}s".Trim('0'); ;
            }
        }

        public string TimeDisplay {
            get {
                var time = StartUtc.ToLocalTime();

                if (time.Date == DateTime.Today) {
                    return "Today at " + time.ToString("HH:mm");
                }

                if (time.Date == DateTime.Today.AddDays(-1)) {
                    return "Yesterday at " + time.ToString("HH:mm");
                }

                return $"{time:MMMM dd, yyyy} at {time:HH:mm}";
            }
        }

        public RideDto() {
            Locations = new List<RideLocationDto>();
            Jumps = new List<JumpDto>();
            SegmentAttempts = new List<SegmentAttemptOverviewDto>();
        }
    }
}
