using GeoCoordinatePortable;
using MtbMate.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MtbMate.Utilities
{
    public static class PolyUtils
    {
        public static bool HasPointOnLine(this IList<LatLongModel> path, LocationModel point, int toleranceInMetres) {
            var p = GetModel(point);

            for (int i = 0; i < path.Count - 1; i++) {
                var geo1 = GetModel(path[i]);
                var geo2 = GetModel(path[i + 1]);

                if (Math.Round(geo1.GetDistanceTo(p) + geo2.GetDistanceTo(p), toleranceInMetres) == Math.Round(geo1.GetDistanceTo(geo2), toleranceInMetres)) {
                    return true;
                }
            }

            return false;
        }

        public static double CalculateDistanceKm(this IList<LatLongModel> path) {
            double totalKm = 0;

            for (int i = 0; i < path.Count - 1; i++) {
                var pin1 = GetModel(path[i]);
                var pin2 = GetModel(path[i + 1]);

                var km = pin1.GetDistanceTo(pin2) / 1000;
                totalKm += km;
            }

            return totalKm;
        }

        public static double CalculateDistanceKm(this IList<LocationModel> path) {
            return path.Select(i => i.LatLong)
                .ToList()
                .CalculateDistanceKm();
        }

        public static double CalculateDistanceMi(this IList<LatLongModel> path) {
            return path.CalculateDistanceKm() * 0.621371192;
        }

        public static double CalculateDistanceMi(this IList<LocationModel> path) {
            return path.CalculateDistanceKm() * 0.621371192;
        }

        private static GeoCoordinate GetModel(LatLongModel model) {
            return new GeoCoordinate(model.Latitude, model.Longitude);
        }

        private static GeoCoordinate GetModel(LocationModel model) {
            return GetModel(model.LatLong);
        }
    }
}
