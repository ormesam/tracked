using System.Collections.Generic;
using System.Linq;
using Shared.Interfaces;
using Xamarin.Forms.Maps;

namespace Tracked.Models {
    public class MapPolyline : Polyline {
        public int ZIndex { get; set; } = 1;
        public IEnumerable<ILatLng> Positions { get; set; } = new List<ILatLng>();

        public void GenerateGeoPath() {
            if (Geopath.Any()) {
                return;
            }

            foreach (var position in Positions) {
                Geopath.Add(new Position(position.Latitude, position.Longitude));
            }
        }
    }
}
