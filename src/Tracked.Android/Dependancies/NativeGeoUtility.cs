using Tracked.Dependancies;
using Tracked.Droid.Dependancies;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(NativeGeoUtility))]
namespace Tracked.Droid.Dependancies {
    public class NativeGeoUtility : INativeGeoUtility {
        private MainActivity mainActivity;

        public NativeGeoUtility() {
            mainActivity = CrossCurrentActivity.Current.Activity as MainActivity;
        }

        public void Start() {
            mainActivity.Service.StartForegroundService();
        }

        public void Stop() {
            mainActivity.Service.RemoveForegroundService();
        }
    }
}