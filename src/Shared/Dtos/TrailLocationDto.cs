using Shared.Interfaces;

namespace Shared.Dtos {
    public class TrailLocationDto : ILatLng {
        public int? TrailLocationId { get; set; }
        public int? TrailId { get; set; }
        public int Order { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
