using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Tracked.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Segment {
        [JsonProperty]
        public int? SegmentId { get; set; }
        [JsonProperty]
        public Guid? Id { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public IList<SegmentLocation> Points { get; set; }
        public SegmentLocation Start => Points.FirstOrDefault();
        public SegmentLocation End => Points.LastOrDefault();
        public string DisplayName => Name;

        public Segment() {
            Points = new List<SegmentLocation>();
        }
    }
}
