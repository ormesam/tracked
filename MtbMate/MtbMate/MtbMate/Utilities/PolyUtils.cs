using GeoCoordinatePortable;
using MtbMate.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MtbMate.Utilities
{
    public static class PolyUtils
    {
        public static bool HasPointOnLine(this IList<LatLongModel> path, LatLongModel point, int toleranceInMetres = 25) {
            //var p = GetGeoModel(point);

            foreach (var location in path) {
                // gets distance in Km so convert to meters
                double distance = location.CalculateDistance(point) * 1000;

                if (distance <= toleranceInMetres) {
                    return true;
                }
            }

            //for (int i = 0; i < path.Count - 1; i++) {
            //    var geo1 = GetGeoModel(path[i]);
            //    var geo2 = GetGeoModel(path[i + 1]);

            //    var distance1 = Math.Round(geo1.GetDistanceTo(p) + geo2.GetDistanceTo(p), toleranceInMetres);
            //    var distance2 = Math.Round(geo1.GetDistanceTo(geo2), toleranceInMetres);

            //    if (distance1 == distance2) {
            //        return true;
            //    }
            //}

            return false;
        }

        public static double CalculateDistanceKm(this IList<LatLongModel> path) {
            double totalKm = 0;

            for (int i = 0; i < path.Count - 1; i++) {
                var pin1 = GetGeoModel(path[i]);
                var pin2 = GetGeoModel(path[i + 1]);

                var km = pin2.GetDistanceTo(pin1) / 1000;
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

        private static GeoCoordinate GetGeoModel(LatLongModel model) {
            return new GeoCoordinate(model.Latitude, model.Longitude);
        }

        public static double CalculateDistance(this LatLongModel latLong1, LatLongModel latLong2) {
            return new List<LatLongModel>() {
                latLong1,
                latLong2,
            }.CalculateDistanceKm();
        }

        public static bool LocationsMatch(SegmentModel segment, IList<LatLongModel> rideLocations) {
            rideLocations = rideLocations
                .Where(i => i.Speed >= 1)
                .ToList();

            bool matchesStart = rideLocations
                .HasPointOnLine(segment.Start);

            bool matchesEnd = rideLocations
                .HasPointOnLine(segment.End);

            if (!matchesStart || !matchesEnd) {
                return false;
            };

            var closestPointToSegmentStart = segment.GetClosestStartPoint(rideLocations);
            var closestPointToSegmentEnd = segment.GetClosestEndPoint(rideLocations);

            if (closestPointToSegmentStart == null || closestPointToSegmentEnd == null) {
                return false;
            }

            int startIdx = rideLocations.IndexOf(closestPointToSegmentStart);
            int endIdx = rideLocations.IndexOf(closestPointToSegmentEnd);

            var filteredRideLocations = rideLocations.ToList().GetRange(startIdx, endIdx - startIdx);

            int matchedPointCount = 0;
            int missedPointCount = 0;

            foreach (var segmentLocation in segment.Points) {
                if (filteredRideLocations.HasPointOnLine(segmentLocation)) {
                    matchedPointCount++;
                } else {
                    missedPointCount++;
                }
            }

            // return true if 60% of the segment points match the ride
            return matchedPointCount >= segment.Points.Count * 0.65;
        }
    }
}
