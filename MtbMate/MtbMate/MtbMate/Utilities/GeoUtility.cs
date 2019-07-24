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

        private LocationModel lastLocation;
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

            LocationModel newLocation = new LocationModel
            {
                Timestamp = DateTime.UtcNow,
                Latitude = latitude,
                Longitude = longitude,
            };

            if (lastLocation != null)
            {
                LocationChanged?.Invoke(new LocationChangedEventArgs
                {
                    Location = new LocationSegmentModel
                    {
                        Start = lastLocation,
                        End = newLocation,
                    },
                });
            }

            lastLocation = newLocation;
        }
    }
}
