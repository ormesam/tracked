using MtbMate.Models;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MtbMate.Utilities
{
    public class GeoUtility
    {
        #region Singleton stuff

        private static GeoUtility instance;
        private static readonly object _lock = new object();

        public static GeoUtility Instance {
            get {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new GeoUtility();
                    }

                    return instance;
                }
            }
        }

        #endregion

        public event SpeedChangedEventHandler SpeedChanged;
        public event LocationChangedEventHandler LocationChanged;

        private GeoUtility()
        {
        }

        public async Task Start()
        {
            DependencyService.Get<INativeGeoUtility>().Start();

            return;

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

            SpeedChanged?.Invoke(new SpeedChangedEventArgs
            {
                MetresPerSecond = 0,
            });

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 3, true, settings);
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            Debug.WriteLine(e.Error);
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {
            LocationChanged?.Invoke(new LocationChangedEventArgs
            {
                Location = new LocationModel
                {
                    Timestamp = e.Position.Timestamp.UtcDateTime,
                    Latitude = e.Position.Latitude,
                    Longitude = e.Position.Longitude,
                    MetresPerSecond = e.Position.Speed,
                }
            });

            SpeedChanged?.Invoke(new SpeedChangedEventArgs
            {
                MetresPerSecond = e.Position.Speed,
            });
        }

        public async Task Stop()
        {
            DependencyService.Get<INativeGeoUtility>().Stop();

            return;

            if (!CrossGeolocator.Current.IsListening)
            {
                return;
            }

            await CrossGeolocator.Current.StopListeningAsync();

            SpeedChanged?.Invoke(new SpeedChangedEventArgs
            {
                MetresPerSecond = 0,
            });

            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }

        public void UpdateLocation(double latitude, double longitude, float speed)
        {
            LocationChanged?.Invoke(new LocationChangedEventArgs
            {
                Location = new LocationModel
                {
                    Timestamp = DateTime.UtcNow,
                    Latitude = latitude,
                    Longitude = longitude,
                    MetresPerSecond = speed,
                }
            });
        }
    }
}
