using System;
using System.Collections.Generic;
using System.Linq;
using Api.Utility;
using DataAccess.Models;
using Shared;
using Shared.Dtos;
using Shared.Interfaces;

namespace Api.Analysers {
    public class TrailAnalyser : IRideAnalyser {
        private readonly int missedPointThreshold = 3;
        private readonly int locationMatchThreshold = 12;
        private IDictionary<int, TrailCache> currentTrailCache;
        private RideDto ride;
        private IEnumerable<TrailAnalysis> allTrails;

        #region Old Analysis

        public void Analyse(ModelDataContext context, int userId, RideDto ride) {
            var trailIds = context.Trails
                .Select(row => row.TrailId)
                .ToArray();

            foreach (int trailId in trailIds) {
                Analyse(context, userId, ride, trailId);
            }
        }

        public void AnalyseTrail(ModelDataContext context, int userId, int trailId) {
            var rideIds = context.Rides
                .Where(row => row.UserId == userId)
                .OrderBy(row => row.StartUtc)
                .Select(row => row.RideId)
                .ToArray();

            foreach (int rideId in rideIds) {
                var ride = RideHelper.GetRideDto(context, rideId, userId);

                Analyse(context, userId, ride, trailId);
            }
        }

        private void Analyse(ModelDataContext context, int userId, RideDto ride, int trailId) {
            var trailLocations = GetTrailLocations(context, trailId);
            var rideLocations = ride.Locations.ToArray();
            var rideJumps = ride.Jumps;

            var result = LocationsMatch(trailLocations.Cast<ILatLng>().ToList(), rideLocations.Cast<ILatLng>().ToList());

            if (!result.MatchesTrail) {
                return;
            }

            TrailAttempt attempt = new TrailAttempt {
                RideId = ride.RideId.Value,
                TrailId = trailId,
                UserId = userId,
                StartUtc = rideLocations[result.StartIdx].Timestamp,
                EndUtc = rideLocations[result.EndIdx].Timestamp,
            };

            attempt.Medal = (int)GetMedal(context, attempt.EndUtc - attempt.StartUtc, trailId);

            context.TrailAttempts.Add(attempt);
            context.SaveChanges();
        }

        private LatLng[] GetTrailLocations(ModelDataContext context, int trailId) {
            return context.TrailLocations
                .Where(row => row.TrailId == trailId)
                .OrderBy(row => row.Order)
                .Select(row => new LatLng {
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                })
                .ToArray();
        }

        public LocationMatchResult LocationsMatch(IList<ILatLng> trailLocations, IList<ILatLng> rideLocations) {
            bool matchesStart = rideLocations
                .HasPointOnLine(trailLocations.First());

            bool matchesEnd = rideLocations
                .HasPointOnLine(trailLocations.Last());

            if (!matchesStart || !matchesEnd) {
                return new LocationMatchResult {
                    MatchesTrail = false,
                };
            };

            var closestPointToTrailStart = GetClosestPoint(rideLocations, trailLocations.First());
            var closestPointToTrailEnd = GetClosestPoint(rideLocations, trailLocations.Last());

            if (closestPointToTrailStart == null || closestPointToTrailEnd == null) {
                return new LocationMatchResult {
                    MatchesTrail = false,
                };
            }

            int startIdx = rideLocations.IndexOf(closestPointToTrailStart);
            int endIdx = rideLocations.IndexOf(closestPointToTrailEnd);

            var filteredRideLocations = rideLocations.ToList().GetRange(startIdx, (endIdx - startIdx) + 1);

            int matchedPointCount = 0;
            int missedPointCount = 0;

            foreach (var trailLocation in trailLocations) {
                if (filteredRideLocations.HasPointOnLine(trailLocation)) {
                    matchedPointCount++;
                } else {
                    missedPointCount++;
                }
            }

            // return true if 90% of the trail points match the ride
            return new LocationMatchResult {
                MatchesTrail = matchedPointCount >= trailLocations.Count * 0.9,
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

        private Medal GetMedal(ModelDataContext context, TimeSpan time, int trailId) {
            var existingAttempts = context.TrailAttempts
                .Where(i => i.TrailId == trailId)
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
            public bool MatchesTrail { get; set; }
            public int StartIdx { get; set; }
            public int EndIdx { get; set; }
        }

        private class LatLng : ILatLng {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        #endregion

        public IList<TrailMatchResult> Analyse(RideDto ride, IEnumerable<TrailAnalysis> trails) {
            var results = new List<TrailMatchResult>();
            currentTrailCache = new Dictionary<int, TrailCache>();
            this.ride = ride;
            this.allTrails = trails;

            Queue<RideLocationDto> locations = new(ride.Locations);

            while (locations.Count > 0) {
                var location = locations.Dequeue();

                CheckCachedTrails(location);
                RemoveUnmatchedTrails();
                CheckNewMatchingTrails(location);
                var completedTrails = CheckForCompletedTrails(location);

                results.AddRange(completedTrails);
            }

            return results;
        }

        private void CheckCachedTrails(RideLocationDto location) {
            var threshold = locationMatchThreshold;// Math.Ceiling(location.AccuracyInMetres);

            foreach (var trail in currentTrailCache) {
                var locations = trail.Value.TrailAnalysis.Locations;
                var closeLocations = locations.Where(i => i.GetDistanceM(location) <= threshold);

                if (closeLocations.Any()) {
                    trail.Value.MissedPoints = 0;

                    foreach (var closeLocation in closeLocations) {
                        trail.Value.LocationsHit.Add(closeLocation);
                    }
                } else {
                    trail.Value.MissedPoints++;
                }
            }
        }

        private void RemoveUnmatchedTrails() {
            var keys = currentTrailCache.Keys.ToList();

            foreach (var key in keys) {
                if (currentTrailCache[key].MissedPoints >= missedPointThreshold) {
                    currentTrailCache.Remove(key);
                }
            }
        }

        private void CheckNewMatchingTrails(RideLocationDto location) {
            var threshold = locationMatchThreshold;// Math.Ceiling(location.AccuracyInMetres);

            var unmatchedTrails = allTrails
                .Where(i => !currentTrailCache.ContainsKey(i.TrailId));

            foreach (var trail in unmatchedTrails) {
                var firstLocation = trail.Locations.First();

                bool matchesFirstLocation = IsWithinThresholdAndClosest(firstLocation, location, threshold);

                if (matchesFirstLocation) {
                    var cache = new TrailCache(trail, ride.Locations.IndexOf(location));
                    cache.LocationsHit.Add(firstLocation);

                    currentTrailCache.Add(trail.TrailId, cache);
                }
            }
        }

        private IEnumerable<TrailMatchResult> CheckForCompletedTrails(RideLocationDto location) {
            var threshold = locationMatchThreshold;// Math.Ceiling(location.AccuracyInMetres);

            var keys = currentTrailCache.Keys.ToList();

            foreach (var key in keys) {
                var cache = currentTrailCache[key];
                var trail = cache.TrailAnalysis;
                var lastLocation = trail.Locations.Last();

                bool completed = IsWithinThresholdAndClosest(lastLocation, location, threshold);

                if (!completed) {
                    continue;
                }

                if (cache.LocationsHitPercent < 90) {
                    continue;
                }

                yield return new TrailMatchResult {
                    TrailId = trail.TrailId,
                    StartIdx = currentTrailCache[key].StartIdx,
                    EndIdx = ride.Locations.IndexOf(location),
                };

                currentTrailCache.Remove(key);
            }
        }

        private bool IsWithinThresholdAndClosest(ILatLng trailLocation, RideLocationDto location, int threshold) {
            double distance = trailLocation.GetDistanceM(location);

            bool isWithinThreshold = distance <= threshold;

            if (!isWithinThreshold) {
                return false;
            }

            int locationIdx = ride.Locations.IndexOf(location);
            var nextLocation = ride.Locations
                .Where(i => i.Timestamp > location.Timestamp)
                .FirstOrDefault();

            return IsClosestPoint(nextLocation, trailLocation, distance);
        }

        private bool IsClosestPoint(RideLocationDto nextLocation, ILatLng trailLocation, double lastDistance) {
            double nextDistance = nextLocation.GetDistanceM(trailLocation);

            return lastDistance < nextDistance;
        }

        public class TrailAnalysis {
            public int TrailId { get; set; }
            public IList<ILatLng> Locations { get; set; }
        }

        public class TrailMatchResult {
            public int TrailId { get; set; }
            public int StartIdx { get; set; }
            public int EndIdx { get; set; }
        }

        private class TrailCache {
            public TrailAnalysis TrailAnalysis { get; set; }
            public HashSet<ILatLng> LocationsHit { get; set; } = new();
            public int MissedPoints { get; set; } = 0;
            public int StartIdx { get; set; }

            public double LocationsHitPercent => ((double)LocationsHit.Count / (double)TrailAnalysis.Locations.Count) * 100;

            public TrailCache(TrailAnalysis trailAnalysis, int startIdx) {
                TrailAnalysis = trailAnalysis;
                StartIdx = startIdx;
            }
        }
    }
}
