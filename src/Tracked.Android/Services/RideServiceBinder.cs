using Android.OS;

namespace Tracked.Droid.Services {
    public class RideServiceBinder : Binder {
        readonly RideService service;

        public RideServiceBinder(RideService service) {
            this.service = service;
        }

        public RideService GetRideService() {
            return service;
        }
    }
}