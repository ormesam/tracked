using MtbMate.Models;

namespace MtbMate.Utilities {
    public class AchievementUtility {


        public void AnalyseRide(Ride ride) {
            foreach (var achievement in Model.Instance.Achievements) {
                if (achievement.Check(ride)) {
                    Model.Instance.SaveAchievementResult(new AchievementResult {
                        AcheivementId = achievement.Id,
                        RideId = ride.Id.Value,
                        Time = achievement.Time.Value,
                    });
                }
            }
        }
    }
}
