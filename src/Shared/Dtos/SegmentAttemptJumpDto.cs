using System;

namespace Shared.Dtos {
    public class SegmentAttemptJumpDto {
        public int SegmentAttemptJumpId { get; set; }
        public int SegmentAttemptId { get; set; }
        public int Number { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Airtime { get; set; }
    }
}
