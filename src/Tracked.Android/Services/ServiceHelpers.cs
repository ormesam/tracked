
using Android.App;
using Android.Content;
using Android.OS;

namespace Tracked.Droid.Services {
    public static class ServiceHelpers {
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