using System.Linq;
using Api.Utility;
using DataAccess;
using DataAccess.Models;
using Shared;
using Shared.Dtos;
using Shared.Interfaces;

namespace Api.Analysers {
    public class StatsAnalyser : IRideAnalyser {
        public void Analyse(Transaction transaction, int userId, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                var locations = context.RideLocations
                    .Where(row => row.RideId == rideId)
                    .Where(row => !row.IsRemoved)
                    .OrderBy(row => row.Timestamp)
                    .Select(row => new RideLocationDto {
                        Mph = row.Mph,
                        Latitude = row.Latitude,
                        Longitude = row.Longitude,
                    })
                    .ToList();

                var ride = context.Rides.Find(rideId);

                ride.AverageSpeedMph = locations.Average(row => row.Mph);
                ride.MaxSpeedMph = locations.Max(row => row.Mph);
                ride.DistanceMiles = DistanceHelpers.GetDistanceMile(locations.Cast<ILatLng>().ToList());
                ride.RouteSvgPath = new SvgBuilder(locations.Cast<ILatLng>()).Build();

                context.SaveChanges();
            }
        }
    }
}
