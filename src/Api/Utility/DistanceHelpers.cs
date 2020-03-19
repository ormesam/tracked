using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Interfaces;

namespace Api.Utility {
    public static class DistanceHelpers {
        public static decimal GetDistanceM(this ILatLng latLng, ILatLng other) {
            var d1 = (double)latLng.Latitude * (Math.PI / 180.0);
            var num1 = (double)latLng.Longitude * (Math.PI / 180.0);
            var d2 = (double)other.Latitude * (Math.PI / 180.0);
            var num2 = (double)other.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return (decimal)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
        }

        public static decimal GetDistanceMile(this IList<ILatLng> path) {
            decimal totalKm = 0;

            for (int i = 0; i < path.Count - 1; i++) {
                var pin1 = path[i];
                var pin2 = path[i + 1];

                decimal km = pin2.GetDistanceM(pin1) / 1000;
                totalKm += km;
            }

            return totalKm * 0.621371192m; ;
        }

        public static bool HasPointOnLine(this IList<ILatLng> path, ILatLng point, int toleranceInMetres = 25) {
            return path.Any(i => i.GetDistanceM(point) <= toleranceInMetres);
        }
    }
}
