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

        public void UpdateLocation(LocationModel newLocation)
        {
            LocationChanged?.Invoke(new LocationChangedEventArgs
            {
                Location = newLocation,
            });
        }
    }
}
