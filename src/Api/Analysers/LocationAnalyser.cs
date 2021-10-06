using System.Linq;
using DataAccess;
using DataAccess.Models;

namespace Api.Analysers {
    public class LocationAnalyser : IRideAnalyser {
        private const double maxAccuracy = 20;

        public void Analyse(Transaction transaction, int userId, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
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
}
