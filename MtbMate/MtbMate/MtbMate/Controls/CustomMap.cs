using System.Collections.Generic;
using MtbMate.Models;
using Xamarin.Forms.Maps;

namespace MtbMate.Controls {
    public class CustomMap : Map {
        public IList<MapLocation> RouteCoordinates { get; set; }
        public bool ShowSpeed { get; set; }

        public CustomMap() {
            RouteCoordinates = new List<MapLocation>();
            MapType = MapType.Street;
            ShowSpeed = false;
        }
    }
}
