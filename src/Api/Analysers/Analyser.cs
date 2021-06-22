using DataAccess.Models;

namespace Api.Analysers {
    public class Analyser {
        public static void AnalyseRide(ModelDataContext context, int userId, int rideId) {
            var analysers = new IRideAnalyser[] {
                new LocationAnalyser(),
                new SpeedAnalyser(),
                new JumpAnalyser(),
                new DistanceAnalyser(),
                new TrailAnalyser(),
            };

            foreach (var analyser in analysers) {
                analyser.Analyse(context, userId, rideId);
            }
        }
    }
}
