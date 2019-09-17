using System.Collections.Generic;
using MtbMate.Models;
using Xamarin.Forms.Maps;

namespace MtbMate.Controls {
    public class CustomMap : Map {
        public IList<Location> RouteCoordinates { get; set; }
        public bool ShowSpeed { get; set; }

        public CustomMap() {
            RouteCoordinates = new List<Location>();
            MapType = MapType.Street;
            ShowSpeed = false;
        }
    }
}
