using Android.OS;

namespace MtbMate.Droid.Services
{
    public class LocationUpdatesServiceBinder : Binder
    {
        readonly LocationUpdatesService service;

        public LocationUpdatesServiceBinder(LocationUpdatesService service)
        {
            this.service = service;
        }

        public LocationUpdatesService GetLocationUpdatesService()
        {
            return service;
        }
    }
}