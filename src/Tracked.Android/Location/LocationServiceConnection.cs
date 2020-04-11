using Android.Content;
using Android.OS;

namespace Tracked.Droid.Location {
    public class LocationServiceConnection : Java.Lang.Object, IServiceConnection {
        public static string Tag => typeof(LocationServiceConnection).FullName;
        public LocationBinder Binder { get; private set; }
        public bool IsConnected { get; private set; }

        MainActivity mainActivity;

        public LocationServiceConnection(MainActivity mainActivity) {
            Binder = null;
            IsConnected = false;
            this.mainActivity = mainActivity;
        }

        public void OnServiceConnected(ComponentName name, IBinder service) {
            Binder = service as LocationBinder;
            IsConnected = Binder != null;
        }

        public void OnServiceDisconnected(ComponentName name) {
            IsConnected = false;
            Binder = null;
        }
    }
}