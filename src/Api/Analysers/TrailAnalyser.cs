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
        public const int TrailAnalyserVersion = 1;

        private readonly int missedPointThreshold = 3;
        private readonly int locationMatchThreshold = 12;
        private IDictionary<int, TrailCache> currentTrailCache;
        private RideDto ride;
        private IEnumerable<TrailAnalysis> allTrails;

        public void Analyse(ModelDataContext context, int userId, int rideId) {
            var trails = context.Trails
                .Select(row => new TrailAnalysis {
                    TrailId = row.TrailId,
                    Locations = row.TrailLocations
                        .OrderBy(i => i.Order)
                        .Select(i => new LatLng {
                            Latitude = i.Latitude,
                            Longitude = i.Longitude,
                        })
                        .Cast<ILatLng>(),
                })
                .ToList();

            Analyse(context, userId, RideHelper.GetRideDto(context, rideId, userId), trails);
        }

        public void AnalyseTrail(ModelDataContext context, int userId, int trailId) {
            var rideIds = context.Rides
                .Where(row => row.UserId == userId)
                .OrderBy(row => row.StartUtc)
                .Select(row => row.RideId)
                .ToArray();

            var trail = context.Trails
                .Where(row => row.TrailId == trailId)
                .Select(row => new TrailAnalysis {
                    TrailId = row.TrailId,
                    Locations = row.TrailLocations
                        .OrderBy(i => i.Order)
                        .Select(i => new LatLng {
                            Latitude = i.Latitude,
                            Longitude = i.Longitude,
                        })
                        .Cast<ILatLng>(),
                })
                .SingleOrDefault();

            if (trail == null) {
                return;
            }

            foreach (int rideId in rideIds) {
                var ride = RideHelper.GetRideDto(context, rideId, userId);

                Analyse(context, userId, ride, new[] { trail });
            }
        }

        private void Analyse(ModelDataContext context, int userId, RideDto ride, IEnumerable<TrailAnalysis> trails) {
            var results = Analyse(ride, trails);

            foreach (var result in results) {
                TrailAttempt attempt = new TrailAttempt {
                    RideId = ride.RideId.Value,
                    TrailId = result.TrailId,
                    UserId = userId,
                    StartUtc = ride.Locations[result.StartIdx].Timestamp,
                    EndUtc = ride.Locations[result.EndIdx].Timestamp,
                    TrailAnalyserVersion = TrailAnalyserVersion,
                };

                attempt.Medal = (int)GetMedal(context, attempt.EndUtc - attempt.StartUtc, result.TrailId);

                context.TrailAttempts.Add(attempt);
                context.SaveChanges();
            }
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

        private class LatLng : ILatLng {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

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
            var threshold = locationMatchThreshold;

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
            var threshold = locationMatchThreshold;

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
            var threshold = locationMatchThreshold;

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
            if (nextLocation == null) {
                return true;
            }

            double nextDistance = nextLocation.GetDistanceM(trailLocation);

            return lastDistance < nextDistance;
        }

        public class TrailAnalysis {
            public int TrailId { get; set; }
            public IEnumerable<ILatLng> Locations { get; set; }
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

            public double LocationsHitPercent => ((double)LocationsHit.Count / (double)TrailAnalysis.Locations.Count()) * 100;

            public TrailCache(TrailAnalysis trailAnalysis, int startIdx) {
                TrailAnalysis = trailAnalysis;
                StartIdx = startIdx;
            }
        }
    }
}
