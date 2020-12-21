using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Interfaces;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Tracked.Screens {
    public abstract class MapViewModelBase : ViewModelBase {
        private CustomMap map;
        private MapType mapType;

        public event EventHandler<MapClickedEventArgs> MapTapped;

        public MapViewModelBase(MainContext context)
            : base(context) {
        }

        protected virtual bool IsReadOnly => false;
        protected virtual ILatLng Centre => new LatLng(57.1499749, -2.1950675);
        protected virtual bool CanChangeMapType => !IsReadOnly;

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
            var pins = GetPins().ToList();

            RotatePins(pins);

            foreach (var pin in pins) {
                if (pin == null) {
                    continue;
                }

                map.Pins.Add(pin);
            }
        }

        private void RotatePins(IList<MapPin> pins) {
            var duplicatePins = pins
                .Select(i => new {
                    i.PinId,
                    i.Position.Latitude,
                    i.Position.Longitude,
                })
                .GroupBy(i => new {
                    i.Latitude,
                    i.Longitude,
                }, i => i.PinId)
                .Where(i => i.Count() > 1)
                .ToList();

            int rotationAngle = 45;

            foreach (var duplicates in duplicatePins) {
                int pinCount = duplicates.Count();
                int rotation = -(pinCount / 2 * rotationAngle) - (rotationAngle / 2);

                foreach (var duplicatePinId in duplicates) {
                    var pin = pins.Single(i => i.PinId == duplicatePinId);

                    pin.Rotation = rotation += rotationAngle;
                }
            }
        }

        private void CreatePolylines() {
            var polylines = GetPolylines().ToList();

            foreach (var polyline in polylines) {
                if (polyline == null) {
                    continue;
                }

                CreatePolyline(polyline);
            }
        }

        public void CreatePolyline(MapPolyline polyline) {
            if (polyline.Positions.Count() <= 1) {
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
    }
}
