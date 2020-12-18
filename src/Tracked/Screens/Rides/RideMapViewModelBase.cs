using System.Collections.Generic;
using System.Linq;
using Shared.Dtos;
using Shared.Interfaces;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Tracked.Screens.Rides {
    public abstract class RideMapViewModelBase : MapViewModelBase {
        public RideMapViewModelBase(MainContext context) : base(context) {
        }

        public abstract RideDto Ride { get; }

        protected override ILatLng Centre => Ride.Locations.Midpoint();

        protected override IEnumerable<MapPin> GetPins() {
            yield return GetMaxSpeedPin();

            foreach (var jumpPin in GetJumpPins()) {
                yield return jumpPin;
            }
        }

        protected override IEnumerable<MapPolyline> GetPolylines() {
            var locations = Ride.Locations;

            var maxSpeed = locations.Max(i => i.Mph);
            var lastColour = Color.Blue;
            var polylineLocations = new List<ILatLng> {
                locations.First(),
            };

            for (int i = 1; i < locations.Count; i++) {
                var colour = GetMaxSpeedColour(locations[i].Mph, maxSpeed);

                if (colour != lastColour) {
                    yield return CreatePolyline(polylineLocations, lastColour);

                    polylineLocations.Clear();

                    polylineLocations.Add(locations[i - 1]);

                    lastColour = colour;
                }

                polylineLocations.Add(locations[i]);
            }

            yield return CreatePolyline(polylineLocations, lastColour);
        }

        private MapPolyline CreatePolyline(IEnumerable<ILatLng> polylineLocations, Color lastColour) {
            return new MapPolyline {
                Colour = lastColour,
                Width = 10,
                Positions = polylineLocations.ToList(),
            };
        }

        private MapPin GetMaxSpeedPin() {
            var maxSpeedLocation = Ride.Locations
                .OrderByDescending(i => i.Mph)
                .First();

            return new MapPin {
                Position = new Position(maxSpeedLocation.Latitude, maxSpeedLocation.Longitude),
                IsSpeedPin = true,
                Label = $"{maxSpeedLocation.Mph:F1} mph",
            };
        }

        private IEnumerable<MapPin> GetJumpPins() {
            return Ride.Jumps
                .Select(i => new MapPin {
                    IsJumpPin = true,
                    Label = $"{i.Airtime} s",
                    Position = new Position(i.Latitude, i.Longitude),
                });
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
    }
}
