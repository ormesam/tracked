using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Tracked.Droid {
    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class SplashActivity : Activity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            StartActivity(typeof(MainActivity));
        }
    }
}