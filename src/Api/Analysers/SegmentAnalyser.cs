using System;
using System.Collections.Generic;
using System.Linq;
using Api.Utility;
using DataAccess.Models;
using Shared;
using Shared.Dtos;
using Shared.Interfaces;

namespace Api.Analysers {
    public class SegmentAnalyser : IRideAnalyser {
        public void Analyse(ModelDataContext context, int userId, RideDto ride) {
            var segmentIds = context.Segment
                .Where(row => row.UserId == userId)
                .Select(row => row.SegmentId)
                .ToArray();

            foreach (int segmentId in segmentIds) {
                Analyse(context, userId, ride, segmentId);
            }
        }

        public void AnalyseSegment(ModelDataContext context, int userId, int segmentId) {
            var rideIds = context.Ride
                .Where(row => row.UserId == userId)
                .OrderBy(row => row.StartUtc)
                .Select(row => row.RideId)
                .ToArray();

            foreach (int rideId in rideIds) {
                var ride = RideHelper.GetRideDto(context, rideId, userId);

                Analyse(context, userId, ride, segmentId);
            }
        }

        private void Analyse(ModelDataContext context, int userId, RideDto ride, int segmentId) {
            var segmentLocations = GetSegmentLocations(context, segmentId);
            var rideLocations = ride.Locations.ToArray();
            var rideJumps = ride.Jumps;

            var result = LocationsMatch(segmentLocations.Cast<ILatLng>().ToList(), rideLocations.Cast<ILatLng>().ToList());

            if (!result.MatchesSegment) {
                return;
            }

            SegmentAttempt attempt = new SegmentAttempt {
                RideId = ride.RideId.Value,
                SegmentId = segmentId,
                UserId = userId,
                StartUtc = rideLocations[result.StartIdx].Timestamp,
                EndUtc = rideLocations[result.EndIdx].Timestamp,
            };

            attempt.Medal = (int)GetMedal(context, attempt.EndUtc - attempt.StartUtc, segmentId);

            context.SegmentAttempt.Add(attempt);
            context.SaveChanges();
        }

        private LatLng[] GetSegmentLocations(ModelDataContext context, int segmentId) {
            return context.SegmentLocation
                .Where(row => row.SegmentId == segmentId)
                .OrderBy(row => row.Order)
                .Select(row => new LatLng {
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                })
                .ToArray();
        }

        public LocationMatchResult LocationsMatch(IList<ILatLng> segmentLocations, IList<ILatLng> rideLocations) {
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

        private ILatLng GetClosestPoint(IList<ILatLng> locations, ILatLng point) {
            ILatLng closestLocation = null;
            double lastDistance = double.MaxValue;

            foreach (var location in locations) {
                double distance = location.GetDistanceM(point);

                if (distance < lastDistance) {
                    lastDistance = distance;
                    closestLocation = location;
                }
            }

            return closestLocation;
        }

        private Medal GetMedal(ModelDataContext context, TimeSpan time, int segmentId) {
            var existingAttempts = context.SegmentAttempt
                .Where(i => i.SegmentId == segmentId)
                .Select(i => new { i.EndUtc, i.StartUtc })
                .ToList()
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
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
