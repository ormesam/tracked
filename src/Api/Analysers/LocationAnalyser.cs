using System.Linq;
using DataAccess.Models;

namespace Api.Analysers {
    public class LocationAnalyser : IRideAnalyser {
        private const double maxAccuracy = 20;

        public void Analyse(ModelDataContext context, int userId, int rideId) {
            var locations = context.RideLocations
                .Where(row => row.RideId == rideId)
                .ToList();

            foreach (var location in locations) {
                if (location.AccuracyInMetres > maxAccuracy) {
                    location.IsRemoved = true;
                    location.RemovalReason = "Accuracy less than " + maxAccuracy;
                }
            }

            context.SaveChanges();
        }
    }
}
