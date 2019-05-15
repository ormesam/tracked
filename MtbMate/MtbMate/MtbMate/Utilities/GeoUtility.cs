using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;

namespace MtbMate.Utilities
{
    public class GeoUtility
    {
        private readonly IDisplay reader;

        public GeoUtility(IDisplay reader)
        {
            this.reader = reader;
        }

        public async Task Start()
        {
            reader.UpdateSpeed(0);

            if (CrossGeolocator.Current.IsListening)
            {
                return;
            }

            var settings = new ListenerSettings
            {
                ActivityType = ActivityType.Fitness,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = false,
                ListenForSignificantChanges = false,
                PauseLocationUpdatesAutomatically = false,
            };

            CrossGeolocator.Current.DesiredAccuracy = 1;
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1, true, settings);

            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            // Debug.WriteLine(e.Error);
            reader.ShowError(e.Error.ToString());
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {
            var position = e.Position;
            var output = "\n";
            output += "Full: Lat: " + position.Latitude + " Long: " + position.Longitude;
            output += "\n" + $"Time: {position.Timestamp}";
            output += "\n" + $"Heading: {position.Heading}";
            output += "\n" + $"Speed: {position.Speed}";
            output += "\n" + $"Accuracy: {position.Accuracy}";
            output += "\n" + $"Altitude: {position.Altitude}";
            output += "\n" + $"Altitude Accuracy: {position.AltitudeAccuracy}";
            output += "\n";
            //Debug.WriteLine(output);

            reader.UpdateSpeed((decimal)e.Position.Speed);
            //reader.UpdateSpeed(((decimal?)location.Speed ?? 0m) * 2.237m);
        }

        public async Task Stop()
        {
            if (!CrossGeolocator.Current.IsListening)
            {
                return;
            }

            await CrossGeolocator.Current.StopListeningAsync();

            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;

            reader.UpdateSpeed(0);
        }
    }
}
