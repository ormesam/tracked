using Android.Content;
using Android.OS;

namespace Tracked.Droid.Services
{
    public class CustomServiceConnection : Java.Lang.Object, IServiceConnection
    {
        public MainActivity Activity { get; set; }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            RideServiceBinder binder = (RideServiceBinder)service;
            Activity.Service = binder.GetRideService();
            Activity.Bound = true;
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            Activity.Service = null;
            Activity.Bound = false;
        }
    }
}