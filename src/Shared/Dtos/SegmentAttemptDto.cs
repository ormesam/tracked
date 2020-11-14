using System;

namespace Shared.Dtos {
    public class SegmentAttemptDto {
        public int SegmentAttemptId { get; set; }
        public int RideId { get; set; }
        public int SegmentId { get; set; }
        public string DisplayName { get; set; }
        public Medal Medal { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }

        public TimeSpan Time => EndUtc - StartUtc;
        public string FormattedTime => Time.ToString(@"mm\:ss");
    }
}
