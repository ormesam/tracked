using MtbMate.Models;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MtbMate.Utilities
{
    public class GeoUtility
    {
        private readonly IList<LocationModel> locationModels;

        public GeoUtility()
        {
            locationModels = new List<LocationModel>();
        }

        public async Task Start()
        {
            if (CrossGeolocator.Current.IsListening)
            {
                return;
            }

            var settings = new ListenerSettings
            {
                ActivityType = ActivityType.AutomotiveNavigation,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = false,
                ListenForSignificantChanges = false,
                PauseLocationUpdatesAutomatically = false,
            };

            CrossGeolocator.Current.DesiredAccuracy = 5;

            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(2), 3, true, settings);
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            Debug.WriteLine(e.Error);
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

            Debug.WriteLine(output);

            locationModels.Add(new LocationModel
            {
                Timestamp = e.Position.Timestamp.UtcDateTime,
                Latitude = e.Position.Latitude,
                Longitude = e.Position.Longitude,
            });
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
        }
    }
}
