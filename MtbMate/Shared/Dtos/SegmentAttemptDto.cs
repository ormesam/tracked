using System;

namespace Shared.Dtos {
    public class SegmentAttemptDto {
        public int? SegmentAttemptId { get; set; }
        public int? SegmentId { get; set; }
        public int? RideId { get; set; }
        public Medal Medal { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
    }
}
