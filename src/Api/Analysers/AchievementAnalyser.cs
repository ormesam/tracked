using System.Linq;
using Api.Achievements;
using DataAccess.Models;
using Shared.Dtos;

namespace Api.Analysers {
    public static class AchievementAnalyser {
        public static void AnalyseRideAndSaveAchievements(ModelDataContext context, int rideId, int userId, RideDto model) {
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

            foreach (var speedAchievement in speedAchievements) {
                if (speedAchievement.Check(model)) {
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
                if (jumpAchievement.Check(model)) {
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
