using System;
using System.Diagnostics;
using Shared.Dtos;
using Tracked.Dependancies;
using Tracked.JumpDetection;
using Xamarin.Forms;

namespace Tracked.Utilities {
    public class GeoUtility : IJumpLocationDetector {
        public event LocationChangedEventHandler LocationChanged;
        private bool isRunning;
        private RideLocationDto lastLocation;

        public void AddLocation(RideLocationDto location) {
            Debug.WriteLine(location);

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
