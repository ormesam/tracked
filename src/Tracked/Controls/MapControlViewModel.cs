using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Interfaces;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Screens;
using Tracked.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Tracked.Controls {
    public class MapControlViewModel : ViewModelBase {
        private readonly bool goToMapPageOnClick;
        private readonly string title;
        public readonly bool isReadOnly;
        private readonly bool showRideFeatures;
        private CustomMap map;
        private MapType mapType;

        public event EventHandler<MapClickedEventArgs> MapTapped;

        public MapControlViewModel(
            MainContext context,
            string title,
            IList<MapLocation> locations,
            bool isReadOnly = true,
            bool showRideFeatures = true,
            bool goToMapPageOnClick = true,
            MapType mapType = MapType.Street,
            bool canChangeMapType = false)
            : base(context) {

            this.title = title;
            this.goToMapPageOnClick = goToMapPageOnClick;
            this.isReadOnly = isReadOnly;

            Locations = locations;
            this.showRideFeatures = showRideFeatures;
            MapType = mapType;
            CanChangeMapType = canChangeMapType;
        }

        public CustomMap CreateMap() {
            Position centre = new Position(57.1499749, -2.1950675);

            if (Locations.Any()) {
                var midPoint = Locations.Midpoint();
                centre = new Position(midPoint.Latitude, midPoint.Longitude);
            }

            map = new CustomMap(MapSpan.FromCenterAndRadius(centre, Distance.FromMiles(.5)), isReadOnly);
            map.IsShowingUser = false;
            map.SetBinding(Map.MapTypeProperty, nameof(MapType), BindingMode.TwoWay);
            map.InputTransparent = isReadOnly;
            map.MapClicked += (s, e) => {
                if (!goToMapPageOnClick) {
                    MapTapped?.Invoke(null, e);
                }
            };

            CreatePolylinesFromLocations();

            return map;
        }

        public bool CanChangeMapType { get; }
        public IList<MapLocation> Locations { get; }
        public override string Title => title;

        public MapType MapType {
            get { return mapType; }
            set {
                if (mapType != value) {
                    mapType = value;
                    OnPropertyChanged(nameof(MapType));
                }
            }
        }

        public bool CanMove => !isReadOnly;

        public IEnumerable<MapType> MapTypes => (IEnumerable<MapType>)Enum.GetValues(typeof(MapType));

        private void CreatePolylinesFromLocations() {
            if (Locations.Count <= 1) {
                return;
            }

            // Only need one line if no colours are involved
            if (!showRideFeatures) {
                AddPolyLine(Locations.Cast<ILatLng>().ToList(), Color.Blue);

                return;
            }

            var maxSpeed = Locations.Max(i => i.Mph);
            var lastColour = Color.Blue;
            var polylineLocations = new List<ILatLng> {
                Locations.First(),
            };

            for (int i = 1; i < Locations.Count; i++) {
                var colour = showRideFeatures ? GetMaxSpeedColour(Locations[i].Mph, maxSpeed) : Color.Blue;

                if (colour != lastColour) {
                    AddPolyLine(polylineLocations, lastColour);

                    polylineLocations.Clear();

                    polylineLocations.Add(Locations[i - 1]);

                    lastColour = colour;
                }

                polylineLocations.Add(Locations[i]);

                bool isMaxSpeed = Locations[i].Mph == maxSpeed;
                bool hasJump = Locations[i].Jump != null;
                bool hasMultiplePins = isMaxSpeed && hasJump;

                if (isMaxSpeed) {
                    AddMaxSpeedPin(Locations[i], hasMultiplePins);
                }

                if (hasJump) {
                    AddJumpPin(Locations[i], hasMultiplePins);
                }
            }

            AddPolyLine(polylineLocations, lastColour);
        }

        private async Task GoToMapScreenAsync() {
            if (!goToMapPageOnClick) {
                return;
            }

            await Context.UI.GoToMapScreenAsync(title, Locations, showRideFeatures);
        }

        private void AddMaxSpeedPin(MapLocation location, bool hasMultiplePins) {
            Pin pin = new CustomMapPin {
                Position = new Position(location.Latitude, location.Longitude),
                Label = Math.Round(location.Mph, 1) + " mph",
                IsSpeedPin = true,
                Rotation = hasMultiplePins ? 330 : 0,
            };

            map.Pins.Add(pin);
        }

        private void AddJumpPin(MapLocation location, bool hasMultiplePins) {
            Pin pin = new CustomMapPin {
                Position = new Position(location.Latitude, location.Longitude),
                Label = Math.Round(location.Jump.Airtime, 3) + "s",
                IsJumpPin = true,
                Rotation = hasMultiplePins ? 30 : 0,
            };

            map.Pins.Add(pin);
        }

        public void AddPolyLine(IList<ILatLng> latLngs, Color colour) {
            if (latLngs.Count <= 1) {
                return;
            }

            Polyline polyline = new Polyline();

            foreach (var latLng in latLngs) {
                polyline.Geopath.Add(new Position(latLng.Latitude, latLng.Longitude));
            }

            polyline.StrokeColor = colour;
            polyline.StrokeWidth = 10f;

            map.MapElements.Add(polyline);
        }

        private Color GetMaxSpeedColour(double mph, double maxSpeed) {
            double limit1 = maxSpeed * 0.95;
            double limit2 = maxSpeed * 0.85;
            double limit3 = maxSpeed * 0.75;
            double limit4 = maxSpeed * 0.70;
            double limit5 = maxSpeed * 0.65;
            double limit6 = maxSpeed * 0.60;
            double limit7 = maxSpeed * 0.55;
            double limit8 = maxSpeed * 0.50;

            if (mph > limit1) {
                return Color.FromHex("#F8696B");
            }

            if (mph > limit2) {
                return Color.FromHex("#F98971");
            }

            if (mph > limit3) {
                return Color.FromHex("#FBAA77");
            }

            if (mph > limit4) {
                return Color.FromHex("#FDCA7D");
            }

            if (mph > limit5) {
                return Color.FromHex("#FFEB84");
            }

            if (mph > limit6) {
                return Color.FromHex("#D8E082");
            }

            if (mph > limit7) {
                return Color.FromHex("#B1D580");
            }

            if (mph > limit8) {
                return Color.FromHex("#8ACA7E");
            }

            return Color.FromHex("#63BE7B");
        }

        public async Task OnMappedTapped(object sender, EventArgs e) {
            if (goToMapPageOnClick) {
                await GoToMapScreenAsync();
            }
        }
    }
}
