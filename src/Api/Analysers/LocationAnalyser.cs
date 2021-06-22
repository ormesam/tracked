using System.Linq;
using DataAccess.Models;

namespace Api.Analysers {
    public class LocationAnalyser : IRideAnalyser {
        private double minAccuracy;

        public void Analyse(ModelDataContext context, int userId, int rideId) {
            var locations = context.RideLocations
                .Where(row => row.RideId == rideId)
                .ToList();

            foreach (var location in locations) {
                if (location.AccuracyInMetres > 20) {
                    location.IsRemoved = true;
                    location.RemovalReason = "Accuracy";
                }
            }

            context.SaveChanges();
        }
    }
}
