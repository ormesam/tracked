using System;
using System.Collections.Generic;
using System.Linq;
using Tracked.Models;

namespace Tracked.Utilities {
    public static class PolyUtils {
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
                    .OrderBy(i => Math.Abs((i.Timestamp - jump.Timestamp).TotalSeconds))
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
