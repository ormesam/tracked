using System;
using System.Collections.Generic;
using Essentials.Core.Extensions;

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
        public int? AnalyserVersion { get; set; }
        public IList<RideLocationDto> Locations { get; set; }
        public IList<JumpDto> Jumps { get; set; }
        public IList<TrailAttemptDto> TrailAttempts { get; set; }
        public IList<AchievementDto> Achievements { get; set; }

        public string FormattedTime => (EndUtc - StartUtc).ToReadableString();

        public string TimeDisplay => StartUtc.ToReadableString();

        public RideDto() {
            Locations = new List<RideLocationDto>();
            Jumps = new List<JumpDto>();
            TrailAttempts = new List<TrailAttemptDto>();
            Achievements = new List<AchievementDto>();
        }
    }
}
