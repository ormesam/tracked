using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Shared;
using Shared.Interfaces;

namespace Api.Analysers {
    public static class SegmentAnalyser {
        public static void AnalyseSegment(ModelDataContext context, int userId, int segmentId) {
            var rideIds = context.Ride
                .Where(row => row.UserId == userId)
                .OrderBy(row => row.StartUtc)
                .Select(row => row.RideId)
                .ToArray();

            foreach (int rideId in rideIds) {
                Analyse(context, userId, rideId, segmentId);
            }
        }

        public static void AnalyseRide(ModelDataContext context, int userId, int rideId) {
            var segmentIds = context.Segment
                .Where(row => row.UserId == userId)
                .Select(row => row.SegmentId)
                .ToArray();

            foreach (int segmentId in segmentIds) {
                Analyse(context, userId, rideId, segmentId);
            }
        }

        private static void Analyse(ModelDataContext context, int userId, int rideId, int segmentId) {
            var segmentLocations = AnalyserHelper.GetSegmentLocations(context, segmentId);
            var rideLocations = AnalyserHelper.GetRideLocations(context, rideId);
            var rideJumps = AnalyserHelper.GetRideJumps(context, rideId);

            var result = LocationsMatch(segmentLocations.Cast<ILatLng>().ToList(), rideLocations.Cast<ILatLng>().ToList());

            if (result.MatchesSegment) {
                SegmentAttempt attempt = new SegmentAttempt {
                    RideId = rideId,
                    SegmentId = segmentId,
                    UserId = userId,
                    StartUtc = rideLocations[result.StartIdx].Timestamp,
                    EndUtc = rideLocations[result.EndIdx].Timestamp,
                };

                attempt.Medal = (int)GetMedal(context, attempt.EndUtc - attempt.StartUtc, segmentId);

                context.SegmentAttempt.Add(attempt);
                context.SaveChanges();

                var locations = rideLocations[result.StartIdx..result.EndIdx]
                    .Select(row => new SegmentAttemptLocation {
                        SegmentAttemptId = attempt.SegmentAttemptId,
                        RideLocationId = row.RideLocationId,
                    })
                    .ToList();

                context.SegmentAttemptLocation.AddRange(locations);
                context.SaveChanges();

                int jumpCount = 1;

                var jumps = rideJumps
                    .Where(row => row.Timestamp >= rideLocations[result.StartIdx].Timestamp)
                    .Where(row => row.Timestamp <= rideLocations[result.EndIdx].Timestamp)
                    .Select(row => new SegmentAttemptJump {
                        SegmentAttemptId = attempt.SegmentAttemptId,
                        JumpId = row.JumpId,
                        Number = jumpCount++,
                    })
                    .ToList();

                context.SegmentAttemptJump.AddRange(jumps);
                context.SaveChanges();
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
    }
}
