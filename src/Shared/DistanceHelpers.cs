using System;
using System.Collections.Generic;
using Shared.Interfaces;

namespace Shared {
    public static class DistanceHelpers {
        public static double GetDistanceM(this ILatLng latLng, ILatLng other) {
            var d1 = latLng.Latitude * (Math.PI / 180.0);
            var num1 = latLng.Longitude * (Math.PI / 180.0);
            var d2 = other.Latitude * (Math.PI / 180.0);
            var num2 = other.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public static double GetDistanceMile(this IList<ILatLng> path) {
            double totalKm = 0;

            for (int i = 0; i < path.Count - 1; i++) {
                var pin1 = path[i];
                var pin2 = path[i + 1];

                double km = pin2.GetDistanceM(pin1) / 1000;
                totalKm += km;
            }

            return totalKm * 0.621371192;
        }
    }
}
