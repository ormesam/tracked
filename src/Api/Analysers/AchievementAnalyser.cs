using System.Linq;
using DataAccess.Models;

namespace Api.Analysers {
    public static class AchievementAnalyser {
        public static void AnalyseRide(ModelDataContext context, int userId, int rideId) {
            var speedAchievements = context.SpeedAchievement
                .Select(row => new MinSpeedAchievement {
                    SpeedAchievementId = row.SpeedAchievementId,
                    MinMph = row.MinMph,
                    Name = row.Name,
                })
                .ToList();

            var jumpAchievements = context.JumpAchievement
                .Select(row => new MinJumpAchievement {
                    JumpAchievementId = row.JumpAchievementId,
                    MinAirtime = row.MinAirtime,
                    Name = row.Name,
                })
                .ToArray();

            var rideLocations = AnalyserHelper.GetRideLocations(context, rideId);
            var rideJumps = AnalyserHelper.GetRideJumps(context, rideId);

            foreach (var speedAchievement in speedAchievements) {
                if (speedAchievement.Check(rideLocations)) {
                    UserSpeedAchievement userSpeedAchievement = new UserSpeedAchievement {
                        RideId = rideId,
                        SpeedAchievementId = speedAchievement.SpeedAchievementId,
                        UserId = userId,
                    };

                    context.UserSpeedAchievement.Add(userSpeedAchievement);
                    context.SaveChanges();
                }
            }

            foreach (var jumpAchievement in jumpAchievements) {
                if (jumpAchievement.Check(rideJumps)) {
                    UserJumpAchievement userJumpAchievement = new UserJumpAchievement {
                        RideId = rideId,
                        JumpAchievementId = jumpAchievement.JumpAchievementId,
                        UserId = userId,
                    };

                    context.UserJumpAchievement.Add(userJumpAchievement);
                    context.SaveChanges();
                }
            }
        }
    }
}
