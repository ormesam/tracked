using Shared.Dtos;
using Shared.Interfaces;

namespace Tracked.Models {
    public class MapLocation : ILatLng {
        public double Mph { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public JumpDto Jump { get; set; }
    }
}
