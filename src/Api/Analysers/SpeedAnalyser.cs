using System.Linq;
using DataAccess;
using DataAccess.Models;

namespace Api.Analysers {
    public class SpeedAnalyser : IRideAnalyser {
        public void Analyse(Transaction transaction, int userId, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                var speedAchievements = context.SpeedAchievements
                    .Select(row => new MinSpeedAchievement {
                        SpeedAchievementId = row.SpeedAchievementId,
                        MinMph = row.MinMph,
                        Name = row.Name,
                    })
                    .ToList();

                var speeds = context.RideLocations
                    .Where(row => row.RideId == rideId)
                    .Where(row => !row.IsRemoved)
                    .Select(row => row.Mph)
                    .ToList();

                foreach (var speedAchievement in speedAchievements) {
                    if (speedAchievement.Check(speeds)) {
                        UserSpeedAchievement userSpeedAchievement = new UserSpeedAchievement {
                            RideId = rideId,
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
}
