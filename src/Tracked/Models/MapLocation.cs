using Shared.Dtos;
using Shared.Interfaces;

namespace Tracked.Models {
    public class MapLocation : ILatLng {
        public decimal Mph { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public RideJumpDto Jump { get; set; }
    }
}
