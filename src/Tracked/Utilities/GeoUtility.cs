using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Shared.Dtos;
using Tracked.Dependancies;
using Tracked.JumpDetection;
using Xamarin.Forms;

namespace Tracked.Utilities {
    public class GeoUtility : IJumpLocationDetector {
        #region Singleton stuff

        private static GeoUtility instance;
        private static readonly object _lock = new object();

        public static GeoUtility Instance {
            get {
                lock (_lock) {
                    if (instance == null) {
                        instance = new GeoUtility();
                    }

                    return instance;
                }
            }
        }

        #endregion

        public event LocationChangedEventHandler LocationChanged;

        private RideLocationDto lastLocation;

        private GeoUtility() {
            CrossGeolocator.Current.DesiredAccuracy = 0;
            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
        }

        private void Current_PositionChanged(object sender, PositionEventArgs e) {
            var position = e.Position;

            if (position.Accuracy > 20) {
                return;
            }

            var location = new RideLocationDto {
                Timestamp = position.Timestamp.UtcDateTime,
                Latitude = (decimal)position.Latitude,
                Longitude = (decimal)position.Longitude,
                AccuracyInMetres = (decimal)position.Accuracy,
                SpeedMetresPerSecond = (decimal)position.Speed,
                Altitude = (decimal)position.Altitude,
            };

            lastLocation = location;

            LocationChanged?.Invoke(new LocationChangedEventArgs {
                Location = location
            });
        }

        public async Task Start() {
            DependencyService.Get<INativeGeoUtility>().Start();

            var listenerSettings = new ListenerSettings {
                ActivityType = ActivityType.Fitness,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = false,
                ListenForSignificantChanges = false,
                PauseLocationUpdatesAutomatically = false,
            };

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1, false, listenerSettings);
        }

        public async Task Stop() {
            DependencyService.Get<INativeGeoUtility>().Stop();

            await CrossGeolocator.Current.StopListeningAsync();

            lastLocation = null;
        }

        public RideLocationDto GetLastLocation(DateTime time) {
            return lastLocation;
        }
    }
}
