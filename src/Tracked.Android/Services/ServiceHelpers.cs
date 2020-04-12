using Android.App;
using Android.Content;
using Android.OS;
using Tracked.Droid.Location;

namespace Tracked.Droid.Services {
    public static class ServiceHelpers {
        public static void StartLocationUpdates(this MainActivity context, Bundle args = null) {
            context.StartForegroundServiceCompat<LocationService>(args);

            context.LocationServiceConnection.Binder.LocationService.StartLocationUpdates();
        }

        public static void StopLocationUpdates(this MainActivity context, Bundle args = null) {
            context.LocationServiceConnection.Binder.LocationService.StopLocationUpdates();

            context.StopForegroundServiceCompat<LocationService>(args);
        }

        public static void StartForegroundServiceCompat<T>(this Context context, Bundle args = null) where T : Service {
            var intent = new Intent(context, typeof(T));

            if (args != null) {
                intent.PutExtras(args);
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O) {
                context.StartForegroundService(intent);
            } else {
                context.StartService(intent);
            }
        }

        public static void StopForegroundServiceCompat<T>(this Context context, Bundle args = null) where T : Service {
            var intent = new Intent(context, typeof(T));

            if (args != null) {
                intent.PutExtras(args);
            }

            context.StopService(intent);
        }
    }
}