using System.Linq;
using DataAccess.Models;
using Shared;
using Shared.Dtos;

namespace Api.Utility {
    public static class RideHelper {
        public static RideDto GetRideDto(ModelDataContext context, int rideId, int userId) {
            var ride = context.Ride
                .Where(row => row.RideId == rideId)
                .Where(row => row.UserId == userId)
                .Select(row => new RideDto {
                    RideId = row.RideId,
                    StartUtc = row.StartUtc,
                    EndUtc = row.EndUtc,
                    Name = row.Name,
                    AverageSpeedMph = row.AverageSpeedMph,
                    MaxSpeedMph = row.MaxSpeedMph,
                    DistanceMiles = row.DistanceMiles,
                    UserId = row.UserId,
                    UserName = row.User.Name,
                    UserProfileImageUrl = row.User.ProfileImageUrl,
                })
                .SingleOrDefault();

            if (ride == null) {
                return null;
            }

            ride.Jumps = context.Jump
                .Where(row => row.RideId == rideId)
                .OrderBy(row => row.Number)
                .Select(row => new JumpDto {
                    JumpId = row.JumpId,
                    Airtime = row.Airtime,
                    Number = row.Number,
                    Timestamp = row.Timestamp,
                })
                .ToList();

            ride.Locations = context.RideLocation
                .Where(row => row.RideId == rideId)
                .OrderBy(row => row.Timestamp)
                .Select(row => new RideLocationDto {
                    RideLocationId = row.RideLocationId,
                    AccuracyInMetres = row.AccuracyInMetres,
                    Altitude = row.Altitude,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    Mph = row.Mph,
                    Timestamp = row.Timestamp,
                })
                .ToList();

            ride.SegmentAttempts = context.SegmentAttempt
                .Where(row => row.RideId == rideId)
                .Select(row => new SegmentAttemptOverviewDto {
                    SegmentAttemptId = row.SegmentAttemptId,
                    RideId = row.RideId,
                    DisplayName = row.Segment.Name,
                    StartUtc = row.StartUtc,
                    EndUtc = row.EndUtc,
                    Medal = (Medal)row.Medal,
                })
                .ToList()
                .OrderByDescending(row => row.Time)
                .ToList();

            return ride;
        }
    }
}
