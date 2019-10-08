using System.Linq;
using System.Threading.Tasks;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public class AchievementUtility {
        public async Task ReanalyseAchievementResults() {
            Model.Instance.RemoveAchievementResults();

            foreach (var ride in Model.Instance.Rides.OrderBy(i => i.Start)) {
                await AnalyseRide(ride);
            }
        }

        public async Task AnalyseRide(Ride ride) {
            foreach (var achievement in Model.Instance.Achievements) {
                if (achievement.Check(ride)) {
                    achievement.IsAchieved = true;
                    achievement.RideId = ride.Id.Value;
                    achievement.Time = ride.Start.Value;

                    await Model.Instance.SaveAchievementResult(new AchievementResult {
                        AcheivementId = achievement.Id,
                        RideId = ride.Id.Value,
                        Time = ride.Start.Value,
                    });
                }
            }
        }
    }
}
