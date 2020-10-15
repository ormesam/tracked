using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoordinateSharp;
using Shared.Interfaces;

namespace Api.Utility {
    public class SvgBuilder {
        private readonly int svgWidth = 1024;
        private readonly IEnumerable<ILatLng> originalLocations;
        private readonly IList<Point> positions = new List<Point>();
        private int svgHeight;
        private (double x, double y) topLeft;
        private (double x, double y) bottomRight;

        public SvgBuilder(IEnumerable<ILatLng> locations) {
            this.originalLocations = locations;
        }

        public (int width, int height, string path) Build() {
            foreach (var location in originalLocations) {
                var coord = new Coordinate(location.Latitude, location.Longitude);

                positions.Add(new Point(coord.UTM.Easting, coord.UTM.Northing));
            }

            CalculateBoundaries();
            CalculatePositions();

            return (svgWidth, svgHeight, CreateSvg());
        }

        private void CalculateBoundaries() {
            topLeft = (positions.Min(i => i.X), positions.Min(i => i.Y));
            bottomRight = (positions.Max(i => i.X), positions.Max(i => i.Y));

            var width = bottomRight.x - topLeft.x;
            var height = bottomRight.y - topLeft.y;

            var aspectRatio = height / width;

            this.svgHeight = (int)(this.svgWidth * aspectRatio);
        }

        private void CalculatePositions() {
            for (int i = 0; i < positions.Count; i++) {
                var position = positions[i];

                var percentageX = (position.X - topLeft.x) / (bottomRight.x - topLeft.x);
                var percentageY = (position.Y - topLeft.y) / (bottomRight.y - topLeft.y);

                positions[i].X = (int)(svgWidth * percentageX);
                positions[i].Y = (int)(svgHeight - (svgHeight * percentageY));
            }
        }

        private string CreateSvg() {
            var start = positions.First();

            StringBuilder sb = new StringBuilder();
            sb.Append($"M {start.X},{start.Y}");

            foreach (var position in positions) {
                sb.Append($" L {position.X},{position.Y}");
            }

            return sb.ToString();
        }

        private class Point {
            public double X { get; set; }
            public double Y { get; set; }

            public Point(double easting, double northing) {
                this.X = easting;
                this.Y = northing;
            }
        }
    }
}
