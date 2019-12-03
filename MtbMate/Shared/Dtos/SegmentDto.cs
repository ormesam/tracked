using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class SegmentDto {
        public int? SegmentId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public IEnumerable<SegmentLocationDto> Locations { get; set; }
    }
}
