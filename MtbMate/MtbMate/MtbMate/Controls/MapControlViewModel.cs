using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Screens;
using MtbMate.Utilities;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MtbMate.Controls {
    public class MapControlViewModel : ViewModelBase {
        private bool goToMapPageOnClick;
        private string title;
        private Map map;
        private MapType mapType;
        private bool isReadOnly;
        private bool isShowingUser;

        public event EventHandler<MapClickedEventArgs> MapTapped;

        public bool ShowSpeed { get; }
        public bool CanChangeMapType { get; }
        public CameraUpdate InitalCamera { get; }
        public IList<MapLocation> Locations { get; }
        public override string Title => title;

        public MapControlViewModel(
            MainContext context,
            string title,
            IList<MapLocation> locations,
            bool isReadOnly = true,
            bool showSpeed = true,
            bool isShowingUser = false,
            bool goToMapPageOnClick = true,
            MapType mapType = MapType.Street,
            bool canChangeMapType = false)
            : base(context) {

            this.title = title;
            this.goToMapPageOnClick = goToMapPageOnClick;
            this.isReadOnly = isReadOnly;
            this.isShowingUser = isShowingUser;

            Locations = locations;
            ShowSpeed = showSpeed;
            MapType = mapType;
            CanChangeMapType = canChangeMapType;

            LatLng centre = new LatLng(57.1499749, -2.1950675);
            double zoom = 10;

            if (locations.Any()) {
                centre = locations.Midpoint().Point;
                zoom = 15;
            } else {
                var lastLocation = CrossGeolocator.Current.GetLastKnownLocationAsync().Result;

                if (lastLocation != null) {
                    centre = new LatLng(lastLocation.Latitude, lastLocation.Longitude);
                    zoom = 15;
                }
            }

            InitalCamera = CameraUpdateFactory.NewPositionZoom(new Position(centre.Latitude, centre.Longitude), zoom);
        }

        public MapType MapType {
            get { return mapType; }
            set {
                if (mapType != value) {
                    mapType = value;
                    OnPropertyChanged(nameof(MapType));
                }
            }
        }

        public IEnumerable<MapType> MapTypes => (IEnumerable<MapType>)Enum.GetValues(typeof(MapType));

        public void Init(Map map) {
            // set up readonly properties here so we can access the ui settings
            this.map = map;

            this.map.UiSettings.CompassEnabled = !isReadOnly;
            this.map.MyLocationEnabled = isShowingUser;
            this.map.UiSettings.MyLocationButtonEnabled = isShowingUser;
            this.map.UiSettings.RotateGesturesEnabled = !isReadOnly;
            this.map.UiSettings.ScrollGesturesEnabled = !isReadOnly;
            this.map.UiSettings.TiltGesturesEnabled = !isReadOnly;
            this.map.UiSettings.ZoomControlsEnabled = !isReadOnly;
            this.map.UiSettings.ZoomGesturesEnabled = !isReadOnly;

            CreatePolylines();
        }

        private void CreatePolylines() {
            if (Locations.Count <= 1) {
                return;
            }

            var maxSpeed = Locations.Max(i => i.Mph);

            for (int i = 1; i < Locations.Count; i++) {
                var colour = ShowSpeed ? GetMaxSpeedColour(Locations[i].Mph, maxSpeed) : Color.Blue;

                AddPolyLine(new[] { Locations[i - 1].Point, Locations[i].Point }, colour);

                if (ShowSpeed && Locations[i].Mph == maxSpeed) {
                    AddMaxSpeedPin(Locations[i]);
                }
            }
        }

        public async Task OnMapClicked(INavigation nav, MapClickedEventArgs args) {
            if (goToMapPageOnClick) {
                await GoToMapScreenAsync(nav);
            } else {
                MapTapped?.Invoke(null, args);
            }
        }

        private async Task GoToMapScreenAsync(INavigation nav) {
            if (!goToMapPageOnClick) {
                return;
            }

            await Context.UI.GoToMapScreenAsync(nav, title, Locations, ShowSpeed);
        }

        private void AddMaxSpeedPin(MapLocation location) {
            Pin pin = new Pin {
                Position = new Position(location.Point.Latitude, location.Point.Longitude),
                Label = Math.Round(location.Mph, 1) + " mi/h",
                Icon = BitmapDescriptorFactory.FromBundle("speed_icon.png"),
            };

            map.Pins.Add(pin);
        }

        public void AddPolyLine(LatLng[] latLngs, Color colour) {
            if (latLngs.Length <= 1) {
                return;
            }

            Polyline polyline = new Polyline();

            foreach (var latLng in latLngs) {
                polyline.Positions.Add(new Position(latLng.Latitude, latLng.Longitude));
            }

            polyline.StrokeColor = colour;
            polyline.StrokeWidth = 2.5f;

            map.Polylines.Add(polyline);
        }

        private Color GetMaxSpeedColour(double mph, double maxSpeed) {
            double redLimit = maxSpeed * 0.95;
            double orangeLimit = maxSpeed * 0.85;
            double yellowLimit = maxSpeed * 0.75;

            if (mph > redLimit) {
                return Color.Red;
            }

            if (mph > orangeLimit) {
                return Color.Orange;
            }

            if (mph > yellowLimit) {
                return Color.Yellow;
            }

            return Color.Green;
        }
    }
}
