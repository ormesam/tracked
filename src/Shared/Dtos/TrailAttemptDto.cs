using System;

namespace Shared.Dtos {
    public class TrailAttemptDto {
        public int TrailAttemptId { get; set; }
        public int RideId { get; set; }
        public int TrailId { get; set; }
        public string DisplayName { get; set; }
        public Medal Medal { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int AnalyserVersion { get; set; }

        public TimeSpan Time => EndUtc - StartUtc;
        public string FormattedTime => Time.ToString(@"mm\:ss");
        public bool HasMedal => Medal != Medal.None;
    }
}
