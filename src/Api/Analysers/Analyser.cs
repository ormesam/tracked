using Api.Utility;
using DataAccess;
using DataAccess.Models;

namespace Api.Analysers {
    public class Analyser {
        // Increment this everytime there is a change to the logic which would affect previous results
        // This will ask users if they want to re-analyse their trail attempts which used an old algorithm
        public const int AnalyserVersion = 2;

        public static void AnalyseRide(Transaction transaction, int userId, int rideId) {
            RideHelper.ThrowIfNotOwner(transaction, rideId, userId);

            var analysers = new IRideAnalyser[] {
                new LocationAnalyser(),
                new SpeedAnalyser(),
                new JumpAnalyser(),
                new DistanceAnalyser(),
                new TrailAnalyser(),
            };

            foreach (var analyser in analysers) {
                analyser.Analyse(transaction, userId, rideId);
            }

            UpdateRideAnalyserVersion(transaction, rideId);
        }

        private static void UpdateRideAnalyserVersion(Transaction transaction, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                var ride = context.Rides.Find(rideId);
                ride.AnalyserVersion = AnalyserVersion;
                context.SaveChanges();
            }
        }
    }
}
