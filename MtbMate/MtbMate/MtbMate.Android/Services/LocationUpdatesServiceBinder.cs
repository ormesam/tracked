using Android.OS;

namespace MtbMate.Droid.Services
{
    public class LocationUpdatesServiceBinder : Binder
    {
        readonly LocationService service;

        public LocationUpdatesServiceBinder(LocationService service)
        {
            this.service = service;
        }

        public LocationService GetLocationService()
        {
            return service;
        }
    }
}