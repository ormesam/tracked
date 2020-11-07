using System.Collections.Generic;

namespace Shared.Dtos {
    public class SegmentDto {
        public int? SegmentId { get; set; }
        public string Name { get; set; }
        public IList<SegmentLocationDto> Locations { get; set; }
        public IList<SegmentAttemptDto> Attempts { get; set; }

        public SegmentDto() {
            Locations = new List<SegmentLocationDto>();
            Attempts = new List<SegmentAttemptDto>();
        }
    }
}
