using Android.OS;

namespace Tracked.Droid.Location {
    public class LocationBinder : Binder {
        public LocationService LocationService { get; }

        public LocationBinder(LocationService locationService) {
            LocationService = locationService;
        }
    }
}