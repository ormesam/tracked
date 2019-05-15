using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MtbMate.Utilities
{
    public class GeoUtility
    {
        public async Task GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Console.WriteLine($"Timestamp: {location.Timestamp}, Speed: {location.Speed}, Latitude: {location.Latitude}," +
                        $" Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (Exception ex)
            {
                // TODO

                throw;
            }

        }
    }
}
