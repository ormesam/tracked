using Android.Content;
using Android.OS;

namespace MtbMate.Droid.Services
{
    public class CustomServiceConnection : Java.Lang.Object, IServiceConnection
    {
        public MainActivity Activity { get; set; }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            LocationUpdatesServiceBinder binder = (LocationUpdatesServiceBinder)service;
            Activity.Service = binder.GetLocationUpdatesService();
            Activity.Bound = true;
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            Activity.Service = null;
            Activity.Bound = false;
        }
    }
}