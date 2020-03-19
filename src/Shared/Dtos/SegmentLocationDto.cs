using Shared.Interfaces;

namespace Shared.Dtos {
    public class SegmentLocationDto : ILatLng {
        public int? SegmentLocationId { get; set; }
        public int? SegmentId { get; set; }
        public int Order { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
