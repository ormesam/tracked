using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared.Dtos;
using Shared.Interfaces;

namespace Api.Utility {
    public class SvgBuilder {
        private int width = 1024;
        private int height;
        private IEnumerable<ILatLng> locations;
        private ILatLng topLeft;
        private ILatLng bottomRight;
        private IDictionary<ILatLng, (int x, int y)> positionByLatLng = new Dictionary<ILatLng, (int x, int y)>();

        public SvgBuilder(RideDto ride) {
            this.locations = ride.Locations.Cast<ILatLng>();
        }

        public (int width, int height, string path) Build() {
            CalculateBoundaries();
            CalculatePositions();

            return (width, height, CreateSvg());
        }

        private void CalculateBoundaries() {
            topLeft = new LatLng(locations.Min(i => i.Latitude), locations.Min(i => i.Longitude));
            bottomRight = new LatLng(locations.Max(i => i.Latitude), locations.Max(i => i.Longitude));

            var aspectRatio = (bottomRight.Latitude - topLeft.Latitude) / (bottomRight.Longitude - topLeft.Longitude);

            height = (int)(width * aspectRatio);
        }

        private void CalculatePositions() {
            foreach (var latLng in locations) {
                var percentageX = (latLng.Longitude - topLeft.Longitude) / (bottomRight.Longitude - topLeft.Longitude);
                var percentageY = (latLng.Latitude - topLeft.Latitude) / (bottomRight.Latitude - topLeft.Latitude);

                int x = (int)(width * percentageX);
                int y = (int)(height - (height * percentageY));

                positionByLatLng.Add(latLng, (x, y));
            }
        }

        private string CreateSvg() {
            var start = positionByLatLng[locations.First()];

            StringBuilder sb = new StringBuilder();
            sb.Append($"M {start.x},{start.y}");

            foreach (var latLng in locations) {
                var position = positionByLatLng[latLng];

                sb.Append($" L {position.x},{position.y}");
            }

            return sb.ToString();
        }

        private class LatLng : ILatLng {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double X { get; set; }
            public double Y { get; set; }

            public LatLng(double lat, double lon) {
                this.Latitude = lat;
                this.Longitude = lon;
            }
        }
    }
}
