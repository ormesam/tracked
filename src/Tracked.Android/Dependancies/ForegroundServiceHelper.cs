using Android.Content;
using Tracked.Dependancies;
using Tracked.Droid.Dependancies;
using Tracked.Droid.Location;

[assembly: Xamarin.Forms.Dependency(typeof(ForegroundServiceHelper))]
namespace Tracked.Droid.Dependancies {
    public class ForegroundServiceHelper : INativeForegroundService {
        private static readonly Context context = global::Android.App.Application.Context;

        public void Start() {
            var intent = new Intent(context, typeof(ForegroundService));

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O) {
                context.StartForegroundService(intent);
            } else {
                context.StartService(intent);
            }
        }

        public void Stop() {
            var intent = new Intent(context, typeof(ForegroundService));
            context.StopService(intent);
        }
    }
}