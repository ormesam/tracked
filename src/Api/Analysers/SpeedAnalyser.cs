using System.Linq;
using DataAccess.Models;
using Shared.Dtos;

namespace Api.Analysers {
    public class SpeedAnalyser : IRideAnalyser {
        public void Analyse(ModelDataContext context, int userId, RideDto ride) {
            var speedAchievements = context.SpeedAchievements
                .Select(row => new MinSpeedAchievement {
                    SpeedAchievementId = row.SpeedAchievementId,
                    MinMph = row.MinMph,
                    Name = row.Name,
                })
                .ToList();

            foreach (var speedAchievement in speedAchievements) {
                if (speedAchievement.Check(ride)) {
                    UserSpeedAchievement userSpeedAchievement = new UserSpeedAchievement {
                        RideId = ride.RideId.Value,
                        SpeedAchievementId = speedAchievement.SpeedAchievementId,
                        UserId = userId,
                    };

                    context.UserSpeedAchievements.Add(userSpeedAchievement);
                    context.SaveChanges();
                }
            }
        }
    }
}
