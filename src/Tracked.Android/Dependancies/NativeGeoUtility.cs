using Plugin.CurrentActivity;
using Tracked.Dependancies;
using Tracked.Droid.Dependancies;
using Tracked.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(NativeGeoUtility))]
namespace Tracked.Droid.Dependancies {
    public class NativeGeoUtility : INativeGeoUtility {
        private MainActivity mainActivity;

        public NativeGeoUtility() {
            mainActivity = CrossCurrentActivity.Current.Activity as MainActivity;
        }

        public void Start() {
            mainActivity.StartLocationUpdates();
        }

        public void Stop() {
            mainActivity.StopLocationUpdates();
        }
    }
}