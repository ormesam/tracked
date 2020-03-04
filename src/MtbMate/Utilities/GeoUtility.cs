using System;
using System.Threading.Tasks;
using MtbMate.Dependancies;
using MtbMate.JumpDetection;
using MtbMate.Models;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace MtbMate.Utilities {
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

        private Location lastLocation;

        private GeoUtility() {
            CrossGeolocator.Current.DesiredAccuracy = 0;
            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
        }

        private void Current_PositionChanged(object sender, PositionEventArgs e) {
            var position = e.Position;

            if (position.Accuracy > 20) {
                return;
            }

            var location = new Location {
                Timestamp = position.Timestamp.UtcDateTime,
                Point = new LatLng(position.Latitude, position.Longitude),
                AccuracyInMetres = position.Accuracy,
                SpeedMetresPerSecond = position.Speed,
                Altitude = position.Altitude,
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

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 2, false, listenerSettings);
        }

        public async Task Stop() {
            DependencyService.Get<INativeGeoUtility>().Stop();

            await CrossGeolocator.Current.StopListeningAsync();

            lastLocation = null;
        }

        public Location GetLastLocation(DateTime time) {
            return lastLocation;
        }
    }
}
