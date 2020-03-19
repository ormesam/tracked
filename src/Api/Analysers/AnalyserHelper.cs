using System;
using System.Linq;
using DataAccess.Models;
using Shared.Interfaces;

namespace Api.Analysers {
    public static class AnalyserHelper {
        internal static RideJumpAnalysis[] GetRideJumps(ModelDataContext context, int rideId) {
            return context.Jump
                .Where(row => row.RideId == rideId)
                .OrderBy(row => row.Timestamp)
                .Select(row => new RideJumpAnalysis {
                    JumpId = row.JumpId,
                    Airtime = row.Airtime,
                    Timestamp = row.Timestamp,
                })
                .ToArray();
        }

        internal static RideLocationAnalysis[] GetRideLocations(ModelDataContext context, int rideId) {
            return context.RideLocation
                .Where(row => row.RideId == rideId)
                .OrderBy(row => row.Timestamp)
                .Select(row => new RideLocationAnalysis {
                    RideLocationId = row.RideLocationId,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    SpeedMetresPerSecond = row.SpeedMetresPerSecond,
                    Timestamp = row.Timestamp,
                })
                .ToArray();
        }

        internal static LatLng[] GetSegmentLocations(ModelDataContext context, int segmentId) {
            return context.SegmentLocation
                .Where(row => row.SegmentId == segmentId)
                .OrderBy(row => row.Order)
                .Select(row => new LatLng {
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                })
                .ToArray();
        }
    }

    internal class LatLng : ILatLng {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    internal class RideLocationAnalysis : ILatLng {
        public int RideLocationId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal SpeedMetresPerSecond { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Mph => SpeedMetresPerSecond * 2.23694m;
    }

    internal class RideJumpAnalysis {
        public int JumpId { get; set; }
        public decimal Airtime { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
