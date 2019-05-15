using System;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;

namespace MtbMate.Utilities
{
    public class GeoUtility
    {
        private readonly IDisplay reader;
        private readonly Timer timer;

        public GeoUtility(IDisplay reader)
        {
            this.reader = reader;
            timer = new Timer(1000);

            timer.Elapsed += Timer_Elapsed;
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await UpdateLocation();
        }

        public void Start()
        {
            reader.UpdateSpeed(0);

            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();

            reader.UpdateSpeed(0);
        }

        private async Task UpdateLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Console.WriteLine($"Timestamp: {location.Timestamp}, Speed: {location.Speed}, Latitude: {location.Latitude}," +
                        $" Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                    //reader.UpdateSpeed(((decimal?)location.Speed ?? 0m) * 2.237m);
                    reader.UpdateSpeed((decimal?)location.Speed ?? 0m);
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
