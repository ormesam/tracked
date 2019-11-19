using System;
using System.Collections.Generic;
using System.Linq;
using GeoCoordinatePortable;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public static class PolyUtils {
        public static bool HasPointOnLine(this IList<LatLng> path, LatLng point, int toleranceInMetres = 25) {
            return path.Any(i => i.CalculateDistance(point) * 1000 <= toleranceInMetres);
        }

        public static double CalculateDistanceKm(this IList<LatLng> path) {
            double totalKm = 0;

            for (int i = 0; i < path.Count - 1; i++) {
                var pin1 = GetGeoModel(path[i]);
                var pin2 = GetGeoModel(path[i + 1]);

                var km = pin2.GetDistanceTo(pin1) / 1000;
                totalKm += km;
            }

            return totalKm;
        }

        public static double CalculateDistanceKm(this IList<Location> path) {
            return path.Select(i => i.Point)
                .ToList()
                .CalculateDistanceKm();
        }

        public static double CalculateDistanceMi(this IList<LatLng> path) {
            return path.CalculateDistanceKm() * 0.621371192;
        }

        public static double CalculateDistanceMi(this IList<Location> path) {
            return path.CalculateDistanceKm() * 0.621371192;
        }

        private static GeoCoordinate GetGeoModel(LatLng model) {
            return new GeoCoordinate(model.Latitude, model.Longitude);
        }

        public static double CalculateDistance(this LatLng latLong1, LatLng latLong2) {
            return new List<LatLng>() {
                latLong1,
                latLong2,
            }.CalculateDistanceKm();
        }

        public static LocationMatchResult LocationsMatch(Segment segment, IList<LatLng> rideLocations) {
            bool matchesStart = rideLocations
                .HasPointOnLine(segment.Start.Point);

            bool matchesEnd = rideLocations
                .HasPointOnLine(segment.End.Point);

            if (!matchesStart || !matchesEnd) {
                return new LocationMatchResult {
                    MatchesSegment = false,
                };
            };

            var closestPointToSegmentStart = segment.GetClosestStartPoint(rideLocations);
            var closestPointToSegmentEnd = segment.GetClosestEndPoint(rideLocations);

            if (closestPointToSegmentStart == null || closestPointToSegmentEnd == null) {
                return new LocationMatchResult {
                    MatchesSegment = false,
                };
            }

            int startIdx = rideLocations.IndexOf(closestPointToSegmentStart);
            int endIdx = rideLocations.IndexOf(closestPointToSegmentEnd);

            var filteredRideLocations = rideLocations.ToList().GetRange(startIdx, (endIdx - startIdx) + 1);

            int matchedPointCount = 0;
            int missedPointCount = 0;

            foreach (var segmentLocation in segment.Points) {
                if (filteredRideLocations.HasPointOnLine(segmentLocation.Point)) {
                    matchedPointCount++;
                } else {
                    missedPointCount++;
                }
            }

            // return true if 90% of the segment points match the ride
            return new LocationMatchResult {
                MatchesSegment = matchedPointCount >= segment.Points.Count * 0.9,
                StartIdx = startIdx,
                EndIdx = endIdx,
            };
        }

        public static IList<MapLocation> GetMapLocations(IRide ride) {
            var locations = ride.Locations
                .OrderBy(i => i.Timestamp)
                .Select(i => new {
                    i.Timestamp,
                    i.Point,
                    i.Mph,
                })
                .ToList();

            var jumpsByLocationTime = new Dictionary<DateTime, Jump>();

            foreach (var jump in ride.Jumps) {
                var nearestLocation = locations
                    .OrderBy(i => Math.Abs((i.Timestamp - jump.Time).TotalSeconds))
                    .FirstOrDefault();

                if (!jumpsByLocationTime.ContainsKey(nearestLocation.Timestamp)) {
                    jumpsByLocationTime.Add(nearestLocation.Timestamp, jump);
                }
            }

            IList<MapLocation> mapLocations = new List<MapLocation>();

            foreach (var location in locations) {
                mapLocations.Add(new MapLocation {
                    Jump = jumpsByLocationTime.ContainsKey(location.Timestamp) ? jumpsByLocationTime[location.Timestamp] : null,
                    Mph = location.Mph,
                    Point = location.Point,
                });
            }

            return mapLocations;
        }

        public static IList<MapLocation> GetMapLocations(IList<SegmentLocation> locations) {
            return locations
                .OrderBy(i => i.Order)
                .Select(i => new MapLocation {
                    Point = i.Point,
                    Mph = 0,
                })
                .ToList();
        }
    }
}
