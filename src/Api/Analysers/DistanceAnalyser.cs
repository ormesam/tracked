using System.Linq;
using DataAccess.Models;
using Shared.Dtos;

namespace Api.Analysers {
    public class DistanceAnalyser : IRideAnalyser {
        public void Analyse(ModelDataContext context, int userId, RideDto ride) {
            var distanceAchievements = context.DistanceAchievements
                .Select(row => new MinDistanceAchievement {
                    DistanceAchievementId = row.DistanceAchievementId,
                    MinDistanceMiles = row.MinDistanceMiles,
                    Name = row.Name,
                })
                .ToList();

            foreach (var distanceAchievement in distanceAchievements) {
                if (distanceAchievement.Check(ride)) {
                    UserDistanceAchievement userDistanceAchievement = new UserDistanceAchievement {
                        RideId = ride.RideId.Value,
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
