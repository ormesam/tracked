using Android.App;
using Android.OS;

namespace MtbMate.Droid {
    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, Label = "Mtb Mate", NoHistory = true)]
    public class SplashActivity : Activity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

#if !DEBUG
            Task.Delay(1500).Wait();
#endif

            StartActivity(typeof(MainActivity));
        }
    }
}