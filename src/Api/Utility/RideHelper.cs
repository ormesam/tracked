using System.Collections.Generic;
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

            ride.Jumps = GetJumps(context, rideId);
            ride.Locations = GetLocations(context, rideId);
            ride.TrailAttempts = GetTrailAttempts(context, rideId);
            ride.Achievements = GetAchievements(context, rideId);

            return ride;
        }

        private static IList<JumpDto> GetJumps(ModelDataContext context, int rideId) {
            return context.Jump
                .Where(row => row.RideId == rideId)
                .OrderBy(row => row.Number)
                .Select(row => new JumpDto {
                    JumpId = row.JumpId,
                    Airtime = row.Airtime,
                    Number = row.Number,
                    Timestamp = row.Timestamp,
                })
                .ToList();
        }

        private static IList<RideLocationDto> GetLocations(ModelDataContext context, int rideId) {
            return context.RideLocation
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
        }

        private static IList<TrailAttemptDto> GetTrailAttempts(ModelDataContext context, int rideId) {
            return context.TrailAttempt
                .Where(row => row.RideId == rideId)
                .Select(row => new TrailAttemptDto {
                    TrailAttemptId = row.TrailAttemptId,
                    TrailId = row.TrailId,
                    RideId = row.RideId,
                    DisplayName = row.Trail.Name,
                    StartUtc = row.StartUtc,
                    EndUtc = row.EndUtc,
                    Medal = (Medal)row.Medal,
                })
                .ToList()
                .OrderByDescending(row => row.Time)
                .ToList();
        }

        private static IList<AchievementDto> GetAchievements(ModelDataContext context, int rideId) {
            var achievements = new List<AchievementDto>();
            achievements.AddRange(GetSpeedAchievements(context, rideId));
            achievements.AddRange(GetJumpAchievements(context, rideId));
            achievements.AddRange(GetDistanceAchievements(context, rideId));

            return achievements;
        }

        private static IEnumerable<AchievementDto> GetSpeedAchievements(ModelDataContext context, int rideId) {
            return context.UserSpeedAchievement
                .Where(row => row.RideId == rideId)
                .Select(row => new AchievementDto {
                    AchievementId = row.SpeedAchievementId,
                    Name = row.SpeedAchievement.Name,
                })
                .ToList();
        }

        private static IEnumerable<AchievementDto> GetJumpAchievements(ModelDataContext context, int rideId) {
            return context.UserJumpAchievement
                .Where(row => row.RideId == rideId)
                .Select(row => new AchievementDto {
                    AchievementId = row.JumpAchievementId,
                    Name = row.JumpAchievement.Name,
                })
                .ToList();
        }

        private static IEnumerable<AchievementDto> GetDistanceAchievements(ModelDataContext context, int rideId) {
            return context.UserDistanceAchievement
                .Where(row => row.RideId == rideId)
                .Select(row => new AchievementDto {
                    AchievementId = row.DistanceAchievementId,
                    Name = row.DistanceAchievement.Name,
                })
                .ToList();
        }
    }
}
