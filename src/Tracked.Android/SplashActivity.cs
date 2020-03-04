using Android.App;
using Android.OS;

namespace Tracked.Droid {
    [Activity(Theme = "@style/MainTheme.Splash", Icon = "@mipmap/ic_launcher", MainLauncher = true, Label = "Tracked Dev", NoHistory = true)]
    public class SplashActivity : Activity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            StartActivity(typeof(MainActivity));
        }
    }
}