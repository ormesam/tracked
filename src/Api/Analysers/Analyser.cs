using DataAccess.Models;
using Shared.Dtos;

namespace Api.Analysers {
    public class Analyser {
        public static void AnalyseRide(ModelDataContext context, int userId, RideDto ride) {
            var analysers = new IRideAnalyser[] {
                new SpeedAnalyser(),
                new JumpAnalyser(),
                new DistanceAnalyser(),
                new TrailAnalyser(),
            };

            foreach (var analyser in analysers) {
                analyser.Analyse(context, userId, ride);
            }
        }
    }
}
