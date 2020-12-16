using System.Collections.Generic;
using Shared.Interfaces;
using Xamarin.Forms;

namespace Tracked.Models {
    public class MapPolyline {
        public IList<ILatLng> Positions { get; set; }
        public Color Colour { get; set; }
        public float Width { get; set; }
    }
}
