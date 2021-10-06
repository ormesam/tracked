using System.Linq;
using DataAccess;
using DataAccess.Models;

namespace Api.Analysers {
    public class JumpAnalyser : IRideAnalyser {
        public void Analyse(Transaction transaction, int userId, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                var jumpAchievements = context.JumpAchievements
                    .Select(row => new MinJumpAchievement {
                        JumpAchievementId = row.JumpAchievementId,
                        MinAirtime = row.MinAirtime,
                        Name = row.Name,
                    })
                    .ToArray();

                var airtimes = context.Jumps
                    .Where(row => row.RideId == rideId)
                    .Select(row => row.Airtime)
                    .ToList();

                foreach (var jumpAchievement in jumpAchievements) {
                    if (jumpAchievement.Check(airtimes)) {
                        UserJumpAchievement userJumpAchievement = new UserJumpAchievement {
                            RideId = rideId,
                            JumpAchievementId = jumpAchievement.JumpAchievementId,
                            UserId = userId,
                        };

                        context.UserJumpAchievements.Add(userJumpAchievement);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
