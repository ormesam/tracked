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
        private bool registerMapClick;
        private string title;

        public bool IsReadOnly { get; }
        public bool IsShowingUser { get; }
        public bool ShowSpeed { get; }
        public CameraUpdate InitalCamera { get; }
        public IList<MapLocation> Locations { get; }
        public IList<Polyline> Polylines { get; }
        public IList<Pin> Pins { get; set; }
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
            this.registerMapClick = registerMapClick;

            Polylines = new List<Polyline>();
            Pins = new List<Pin>();
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

            CreatePolylines(locations);
        }

        private void CreatePolylines(IList<MapLocation> locations) {
            if (!locations.Any()) {
                return;
            }

            var maxSpeed = locations.Max(i => i.Mph);

            IList<LatLng> latLng = new List<LatLng>();
            var lastColour = Color.Blue;
            bool firstRun = true;

            foreach (var location in locations) {
                var thisColour = ShowSpeed ? GetMaxSpeedColour(location.Mph, maxSpeed) : Color.Blue;

                if (firstRun || thisColour != lastColour) {
                    if (!firstRun) {
                        AddLine(latLng.ToArray(), lastColour);
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

            AddLine(latLng.ToArray(), lastColour);
        }

        public async Task GoToMapScreenAsync(INavigation nav) {
            if (!registerMapClick) {
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

            Pins.Add(pin);
        }

        private void AddLine(LatLng[] latLngs, Color colour) {
            Polyline polyline = new Polyline();

            foreach (var latLng in latLngs) {
                polyline.Positions.Add(new Position(latLng.Latitude, latLng.Longitude));
            }

            polyline.StrokeColor = colour;
            polyline.StrokeWidth = 2.5f;

            Polylines.Add(polyline);
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

        public void UpdateMap(Map map) {
            foreach (var polyline in Polylines) {
                map.Polylines.Add(polyline);
            }

            foreach (var pin in Pins) {
                map.Pins.Add(pin);
            }
        }
    }
}
