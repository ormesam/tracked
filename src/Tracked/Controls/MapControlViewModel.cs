using System;
using System.Collections.Generic;
using Shared.Interfaces;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Screens;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Tracked.Controls {
    public abstract class MapControlViewModel : ViewModelBase {
        private CustomMap map;
        private MapType mapType;

        public event EventHandler<MapClickedEventArgs> MapTapped;


        public MapControlViewModel(MainContext context)
            : base(context) {
        }

        protected virtual bool IsReadOnly => false;
        protected virtual ILatLng Centre => new LatLng(57.1499749, -2.1950675);
        protected virtual bool CanChangeMapType => true;

        public CustomMap CreateMap() {
            Position centre = new Position(Centre.Latitude, Centre.Longitude);

            map = new CustomMap(MapSpan.FromCenterAndRadius(centre, Distance.FromMiles(.5)), IsReadOnly);
            map.IsShowingUser = false;
            map.SetBinding(Map.MapTypeProperty, nameof(MapType), BindingMode.TwoWay);
            map.InputTransparent = IsReadOnly;
            map.MapClicked += (s, e) => {
                MapTapped?.Invoke(s, e);
            };

            CreatePolylines();
            CreatePins();

            return map;
        }

        protected abstract IEnumerable<MapPin> GetPins();
        protected abstract IEnumerable<MapPolyline> GetPolylines();

        private void CreatePins() {
            var pins = GetPins();

            foreach (var pin in pins) {
                map.Pins.Add(pin);
            }
        }

        private void CreatePolylines() {
            var polylines = GetPolylines();

            foreach (var polyline in polylines) {
                CreatePolyline(polyline);
            }
        }

        private void CreatePolyline(MapPolyline polyline) {
            if (polyline.Positions.Count <= 1) {
                return;
            }

            Polyline polylineElement = new Polyline();

            foreach (var position in polyline.Positions) {
                polylineElement.Geopath.Add(new Position(position.Latitude, position.Longitude));
            }

            polylineElement.StrokeColor = polyline.Colour;
            polylineElement.StrokeWidth = polyline.Width;

            map.MapElements.Add(polylineElement);
        }

        public IEnumerable<MapType> MapTypes => (IEnumerable<MapType>)Enum.GetValues(typeof(MapType));

        public MapType MapType {
            get { return mapType; }
            set {
                if (mapType != value) {
                    mapType = value;
                    OnPropertyChanged(nameof(MapType));
                }
            }
        }
        /*
        private void CreatePolylines() {
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

        private void AddMaxSpeedPin(MapLocation location, bool hasMultiplePins) {
            Pin pin = new MapPin {
                Position = new Position(location.Latitude, location.Longitude),
                Label = Math.Round(location.Mph, 1) + " mph",
                IsSpeedPin = true,
                Rotation = hasMultiplePins ? 330 : 0,
            };

            Map.Pins.Add(pin);
        }

        private void AddJumpPin(MapLocation location, bool hasMultiplePins) {
            Pin pin = new MapPin {
                Position = new Position(location.Latitude, location.Longitude),
                Label = Math.Round(location.Jump.Airtime, 3) + "s",
                IsJumpPin = true,
                Rotation = hasMultiplePins ? 30 : 0,
            };

            Map.Pins.Add(pin);
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
        }*/
    }
}
