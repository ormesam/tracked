using Api.Utility;
using DataAccess.Models;

namespace Api.Analysers {
    public class Analyser {
        // Increment this everytime there is a change to the logic which would affect previous results
        // This will ask users if they want to re-analyse their trail attempts which used an old algorithm
        public const int AnalyserVersion = 2;

        public static void AnalyseRide(ModelDataContext context, int userId, int rideId) {
            RideHelper.ThrowIfNotOwner(context, rideId, userId);

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

            UpdateRideAnalyserVersion(context, rideId);
        }

        private static void UpdateRideAnalyserVersion(ModelDataContext context, int rideId) {
            var ride = context.Rides.Find(rideId);
            ride.AnalyserVersion = AnalyserVersion;
            context.SaveChanges();
        }
    }
}
