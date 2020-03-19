using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class CreateSegmentAttemptDto {
        public int SegmentAttemptId { get; set; }
        public int SegmentId { get; set; }
        public int RideId { get; set; }
        public Medal Medal { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public IList<SegmentAttemptLocationDto> Locations { get; set; }
        public IList<SegmentAttemptJumpDto> Jumps { get; set; }
        public TimeSpan Time => EndUtc - StartUtc;

        public CreateSegmentAttemptDto() {
            Locations = new List<SegmentAttemptLocationDto>();
            Jumps = new List<SegmentAttemptJumpDto>();
        }
    }
}
