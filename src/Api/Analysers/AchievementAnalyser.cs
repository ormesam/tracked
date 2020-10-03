using DataAccess.Models;
using Shared.Dtos;

namespace Api.Analysers {
    public class AchievementAnalyser {
        public static void AnalyseRide(ModelDataContext context, int userId, RideDto ride) {
            var analysers = new IAchievementAnalyser[] {
                new SpeedAnalyser(),
                new JumpAnalyser(),
            };

            foreach (var analyser in analysers) {
                analyser.Analyse(context, userId, ride);
            }
        }
    }
}
