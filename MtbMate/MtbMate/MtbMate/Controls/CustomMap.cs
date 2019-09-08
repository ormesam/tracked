using System.Collections.Generic;
using MtbMate.Models;
using Xamarin.Forms.Maps;

namespace MtbMate.Controls {
    public class CustomMap : Map {
        public IList<LocationModel> RouteCoordinates { get; set; }
        public bool ShowSpeed { get; set; }

        public CustomMap() {
            RouteCoordinates = new List<LocationModel>();
            MapType = MapType.Street;
            ShowSpeed = false;
        }
    }
}
