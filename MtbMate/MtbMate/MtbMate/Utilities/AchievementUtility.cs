using System.Threading.Tasks;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public class AchievementUtility {
        #region Singleton stuff

        private static AchievementUtility instance;
        private static readonly object _lock = new object();

        public static AchievementUtility Instance {
            get {
                lock (_lock) {
                    if (instance == null) {
                        instance = new AchievementUtility();
                    }

                    return instance;
                }
            }
        }

        #endregion

        private AchievementUtility() {
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
