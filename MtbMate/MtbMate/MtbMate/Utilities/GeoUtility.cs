using MtbMate.Models;
using System;
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

        public void Start()
        {
            DependencyService.Get<INativeGeoUtility>().Start();
        }

        public void Stop()
        {
            DependencyService.Get<INativeGeoUtility>().Stop();
        }

        public void UpdateLocation(double latitude, double longitude, float speed)
        {
            SpeedChanged?.Invoke(new SpeedChangedEventArgs
            {
                MetresPerSecond = speed,
            });

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
