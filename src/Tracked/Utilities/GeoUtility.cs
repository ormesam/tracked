using System;
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
        }

        public void AddLocation(RideLocationDto location) {
            lastLocation = location;

            LocationChanged?.Invoke(new LocationChangedEventArgs {
                Location = location
            });
        }

        public void Start() {
            DependencyService.Get<INativeGeoUtility>().Start();
        }

        public void Stop() {
            DependencyService.Get<INativeGeoUtility>().Stop();

            lastLocation = null;
        }

        public RideLocationDto GetLastLocation(DateTime time) {
            return lastLocation;
        }
    }
}
