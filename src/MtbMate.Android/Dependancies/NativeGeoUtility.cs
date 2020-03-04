using MtbMate.Dependancies;
using MtbMate.Droid.Dependancies;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(NativeGeoUtility))]
namespace MtbMate.Droid.Dependancies {
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