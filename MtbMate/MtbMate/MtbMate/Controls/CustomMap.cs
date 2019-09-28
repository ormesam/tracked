using System.Collections.Generic;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MtbMate.Controls {
    public class CustomMap : Map {

        public static readonly BindableProperty RouteCoordinatesProperty =
            BindableProperty.Create(
                nameof(RouteCoordinates),
                typeof(IEnumerable<MapLocation>),
                typeof(CustomMap),
                null);

        public static readonly BindableProperty ShowSpeedProperty =
            BindableProperty.Create(
                nameof(ShowSpeed),
                typeof(bool),
                typeof(CustomMap),
                defaultValue: false);

        public IEnumerable<MapLocation> RouteCoordinates {
            get { return (IEnumerable<MapLocation>)GetValue(RouteCoordinatesProperty); }
            set { SetValue(RouteCoordinatesProperty, value); }
        }

        public bool ShowSpeed {
            get { return (bool)GetValue(ShowSpeedProperty); }
            set { SetValue(ShowSpeedProperty, value); }
        }

        public CustomMap() : base() {
            MapType = MapType.Street;
            ShowSpeed = false;
        }

        public CustomMap(MapSpan region) : base(region) {
            MapType = MapType.Street;
            ShowSpeed = false;
        }
    }
}
