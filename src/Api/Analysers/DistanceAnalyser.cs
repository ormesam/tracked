using System.Linq;
using DataAccess;
using DataAccess.Models;

namespace Api.Analysers {
    public class DistanceAnalyser : IRideAnalyser {
        public void Analyse(Transaction transaction, int userId, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                var distanceAchievements = context.DistanceAchievements
                    .Select(row => new MinDistanceAchievement {
                        DistanceAchievementId = row.DistanceAchievementId,
                        MinDistanceMiles = row.MinDistanceMiles,
                        Name = row.Name,
                    })
                    .ToList();

                double distance = context.Rides
                    .Where(row => row.RideId == rideId)
                    .Select(row => row.DistanceMiles)
                    .SingleOrDefault();

                foreach (var distanceAchievement in distanceAchievements) {
                    if (distanceAchievement.Check(distance)) {
                        UserDistanceAchievement userDistanceAchievement = new UserDistanceAchievement {
                            RideId = rideId,
                            DistanceAchievementId = distanceAchievement.DistanceAchievementId,
                            UserId = userId,
                        };

                        context.UserDistanceAchievements.Add(userDistanceAchievement);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
