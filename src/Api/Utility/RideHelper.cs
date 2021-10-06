using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Models;
using Shared;
using Shared.Dtos;

namespace Api.Utility {
    public static class RideHelper {
        public static RideDto GetRideDto(Transaction transaction, int rideId, int? userId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                var ride = context.Rides
                    .Where(row => row.RideId == rideId)
                    .Where(row => userId == null || row.UserId == userId)
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
                        AnalyserVersion = row.AnalyserVersion,
                    })
                    .SingleOrDefault();

                if (ride == null) {
                    return null;
                }

                ride.Jumps = GetJumps(transaction, rideId);
                ride.Locations = GetLocations(transaction, rideId);
                ride.TrailAttempts = GetTrailAttempts(transaction, rideId);
                ride.Achievements = GetAchievements(transaction, rideId);

                return ride;
            }
        }

        private static IList<JumpDto> GetJumps(Transaction transaction, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                return context.Jumps
                    .Where(row => row.RideId == rideId)
                    .OrderBy(row => row.Number)
                    .Select(row => new JumpDto {
                        JumpId = row.JumpId,
                        Airtime = row.Airtime,
                        Number = row.Number,
                        Timestamp = row.Timestamp,
                        Latitude = row.Latitude,
                        Longitude = row.Longitude
                    })
                    .ToList();
            }
        }

        private static IList<RideLocationDto> GetLocations(Transaction transaction, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                return context.RideLocations
                    .Where(row => row.RideId == rideId)
                    .Where(row => !row.IsRemoved)
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
        }

        private static IList<TrailAttemptDto> GetTrailAttempts(Transaction transaction, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                return context.TrailAttempts
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
        }

        private static IList<AchievementDto> GetAchievements(Transaction transaction, int rideId) {
            var achievements = new List<AchievementDto>();
            achievements.AddRange(GetSpeedAchievements(transaction, rideId));
            achievements.AddRange(GetJumpAchievements(transaction, rideId));
            achievements.AddRange(GetDistanceAchievements(transaction, rideId));

            return achievements;
        }

        private static IEnumerable<AchievementDto> GetSpeedAchievements(Transaction transaction, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                return context.UserSpeedAchievements
                    .Where(row => row.RideId == rideId)
                    .Select(row => new AchievementDto {
                        AchievementId = row.SpeedAchievementId,
                        Name = row.SpeedAchievement.Name,
                    })
                    .ToList();
            }
        }

        private static IEnumerable<AchievementDto> GetJumpAchievements(Transaction transaction, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                return context.UserJumpAchievements
                    .Where(row => row.RideId == rideId)
                    .Select(row => new AchievementDto {
                        AchievementId = row.JumpAchievementId,
                        Name = row.JumpAchievement.Name,
                    })
                    .ToList();
            }
        }

        private static IEnumerable<AchievementDto> GetDistanceAchievements(Transaction transaction, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                return context.UserDistanceAchievements
                    .Where(row => row.RideId == rideId)
                    .Select(row => new AchievementDto {
                        AchievementId = row.DistanceAchievementId,
                        Name = row.DistanceAchievement.Name,
                    })
                    .ToList();
            }
        }

        public static void ThrowIfNotOwner(Transaction transaction, int rideId, int userId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                var exists = context.Rides
                    .Where(row => row.RideId == rideId)
                    .Where(row => row.UserId == userId)
                    .Any();

                if (!exists) {
                    throw new Exception("Unable to perform this operation as the current user");
                }
            }
        }
    }
}
