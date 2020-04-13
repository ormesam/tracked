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

        private bool isRunning;
        private RideLocationDto lastLocation;

        private GeoUtility() {
        }

        public void AddLocation(RideLocationDto location) {
            System.Diagnostics.Debug.WriteLine(location);

            lastLocation = location;

            LocationChanged?.Invoke(new LocationChangedEventArgs {
                Location = location
            });
        }

        public void Start() {
            if (isRunning) {
                return;
            }

            isRunning = true;

            DependencyService.Get<INativeGeoUtility>().Start();
        }

        public void Stop() {
            if (!isRunning) {
                return;
            }

            DependencyService.Get<INativeGeoUtility>().Stop();

            isRunning = false;
            lastLocation = null;
        }

        public RideLocationDto GetLastLocation(DateTime time) {
            return lastLocation;
        }
    }
}
