using Shared.Interfaces;

namespace Shared.Dtos {
    public class SegmentLocationDto : ILatLng {
        public int? SegmentLocationId { get; set; }
        public int? SegmentId { get; set; }
        public int Order { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
