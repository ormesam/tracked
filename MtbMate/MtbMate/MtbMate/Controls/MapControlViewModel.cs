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

        public event EventHandler<MapClickedEventArgs> MapTapped;

        public bool IsReadOnly { get; }
        public bool IsShowingUser { get; }
        public bool ShowSpeed { get; }
        public CameraUpdate InitalCamera { get; }
        public IList<MapLocation> Locations { get; }
        public override string Title => title;

        public MapControlViewModel(
            MainContext context,
            string title,
            IList<MapLocation> locations,
            bool isReadonly = true,
            bool showSpeed = true,
            bool isShowingUser = false,
            bool registerMapClick = true)
            : base(context) {

            this.title = title;
            this.goToMapPageOnClick = registerMapClick;

            Locations = locations;
            IsReadOnly = isReadonly;
            IsShowingUser = isShowingUser;
            ShowSpeed = showSpeed;

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

        public void Init(Map map) {
            this.map = map;

            CreatePolylines();
        }

        private void CreatePolylines() {
            if (!Locations.Any()) {
                return;
            }

            var maxSpeed = Locations.Max(i => i.Mph);

            IList<LatLng> latLng = new List<LatLng>();
            var lastColour = Color.Blue;
            bool firstRun = true;

            foreach (var location in Locations) {
                var thisColour = ShowSpeed ? GetMaxSpeedColour(location.Mph, maxSpeed) : Color.Blue;

                if (firstRun || thisColour != lastColour) {
                    if (!firstRun) {
                        AddPolyLine(latLng.ToArray(), lastColour);
                    }

                    firstRun = false;

                    lastColour = thisColour;

                    var lastLatLon = latLng.LastOrDefault();

                    latLng.Clear();

                    if (lastLatLon != null) {
                        latLng.Add(lastLatLon);
                    }
                }

                latLng.Add(location.Point);

                if (ShowSpeed && location.Mph == maxSpeed) {
                    AddMaxSpeedPin(location);
                }
            }

            AddPolyLine(latLng.ToArray(), lastColour);
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
                // Icon = BitmapDescriptorFactory.from(Resource.Drawable.speed_icon)
            };

            map.Pins.Add(pin);
        }

        public void AddPolyLine(LatLng[] latLngs, Color colour) {
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
