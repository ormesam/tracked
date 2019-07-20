using MtbMate.Droid.Dependancies;
using MtbMate.Utilities;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(NativeGeoUtility))]
namespace MtbMate.Droid.Dependancies
{
    public class NativeGeoUtility : INativeGeoUtility
    {
        private MainActivity mainActivity;

        public NativeGeoUtility()
        {
            mainActivity = CrossCurrentActivity.Current.Activity as MainActivity;
        }

        public void Start()
        {
            mainActivity.Service.RequestLocationUpdates();
        }

        public void Stop()
        {
            mainActivity.Service.RemoveLocationUpdates();
        }
    }
}