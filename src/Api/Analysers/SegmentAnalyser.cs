using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Shared;
using Shared.Dtos;
using Shared.Interfaces;

namespace Api.Analysers {
    public static class SegmentAnalyser {
        public static IEnumerable<SegmentAttemptDto> GetMatchingSegments(ModelDataContext context, RideDto ride) {
            var segments = context.SegmentLocation
                .OrderBy(i => i.SegmentId)
                .ThenBy(i => i.Order)
                .ToLookup(i => i.SegmentId, i => new LatLng {
                    Latitude = i.Latitude,
                    Longitude = i.Longitude,
                });

            foreach (var segment in segments) {
                var result = LocationsMatch(segment.Cast<ILatLng>().ToList(), ride.Locations.Cast<ILatLng>().ToList());

                if (result != null) {
                    var locations = ride.Locations.ToArray()[result.StartIdx..result.EndIdx]
                        .Select(i => new SegmentAttemptLocationDto {
                            AccuracyInMetres = i.AccuracyInMetres,
                            Altitude = i.Altitude,
                            Latitude = i.Latitude,
                            Longitude = i.Longitude,
                            SpeedMetresPerSecond = i.SpeedMetresPerSecond,
                            Timestamp = i.Timestamp,
                        })
                        .ToList();

                    var segmentAttempt = new SegmentAttemptDto {
                        SegmentId = segment.Key,
                        StartUtc = locations.First().Timestamp,
                        EndUtc = locations.Last().Timestamp,
                        Locations = locations,
                    };

                    segmentAttempt.Medal = GetMedal(context, segmentAttempt.Time, segment.Key);

                    yield return segmentAttempt;
                }
            }
        }

        public static LocationMatchResult LocationsMatch(IList<ILatLng> segmentLocations, IList<ILatLng> rideLocations) {
            bool matchesStart = rideLocations
                .HasPointOnLine(segmentLocations.First());

            bool matchesEnd = rideLocations
                .HasPointOnLine(segmentLocations.Last());

            if (!matchesStart || !matchesEnd) {
                return new LocationMatchResult {
                    MatchesSegment = false,
                };
            };

            var closestPointToSegmentStart = GetClosestPoint(rideLocations, segmentLocations.First());
            var closestPointToSegmentEnd = GetClosestPoint(rideLocations, segmentLocations.Last());

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

            foreach (var segmentLocation in segmentLocations) {
                if (filteredRideLocations.HasPointOnLine(segmentLocation)) {
                    matchedPointCount++;
                } else {
                    missedPointCount++;
                }
            }

            // return true if 90% of the segment points match the ride
            return new LocationMatchResult {
                MatchesSegment = matchedPointCount >= segmentLocations.Count * 0.9,
                StartIdx = startIdx,
                EndIdx = endIdx,
            };
        }

        private static ILatLng GetClosestPoint(IList<ILatLng> locations, ILatLng point) {
            ILatLng closestLocation = null;
            decimal lastDistance = decimal.MaxValue;

            foreach (var location in locations) {
                decimal distance = location.GetDistanceM(point);

                if (distance < lastDistance) {
                    lastDistance = distance;
                    closestLocation = location;
                }
            }

            return closestLocation;
        }

        private static Medal GetMedal(ModelDataContext context, TimeSpan time, int segmentId) {
            var existingAttempts = context.SegmentAttempt
                .Where(i => i.SegmentId == segmentId)
                .Select(i => i.EndUtc - i.StartUtc)
                .OrderBy(i => i)
                .ToList();

            if (!existingAttempts.Any()) {
                return Medal.None;
            }

            if (existingAttempts.Count == 1) {
                return time < existingAttempts[0] ? Medal.Gold : Medal.Silver;
            }

            if (existingAttempts.Count == 2) {
                return time < existingAttempts[0] ? Medal.Gold : time < existingAttempts[1] ? Medal.Silver : Medal.Bronze;
            }

            if (time < existingAttempts.FirstOrDefault()) {
                return Medal.Gold;
            } else if (time < existingAttempts.Skip(1).FirstOrDefault()) {
                return Medal.Silver;
            } else if (time < existingAttempts.Skip(2).FirstOrDefault()) {
                return Medal.Bronze;
            }

            return Medal.None;
        }


        public class LocationMatchResult {
            public bool MatchesSegment { get; set; }
            public int StartIdx { get; set; }
            public int EndIdx { get; set; }
        }

        private class LatLng : ILatLng {
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
        }
    }
}
