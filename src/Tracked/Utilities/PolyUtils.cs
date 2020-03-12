using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Dtos;
using Tracked.Models;

namespace Tracked.Utilities {
    public static class PolyUtils {
        public static IList<MapLocation> GetMapLocations(IList<RideLocationDto> rideLocations, IList<JumpDto> rideJumps) {
            var locations = rideLocations
                .OrderBy(i => i.Timestamp)
                .Select(i => new {
                    i.Timestamp,
                    i.Latitude,
                    i.Longitude,
                    i.Mph,
                })
                .ToList();

            var jumpsByLocationTime = new Dictionary<DateTime, JumpDto>();

            foreach (var jump in rideJumps) {
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
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                });
            }

            return mapLocations;
        }

        public static IList<MapLocation> GetMapLocations(IList<SegmentLocationDto> locations) {
            return locations
                .OrderBy(i => i.Order)
                .Select(i => new MapLocation {
                    Latitude = i.Latitude,
                    Longitude = i.Longitude,
                    Mph = 0,
                })
                .ToList();
        }
    }
}
