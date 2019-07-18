using Android.Content;
using Android.Locations;
using Android.Widget;

namespace MtbMate.Droid.Services
{
    public class MyReceiver : BroadcastReceiver
    {
        public Context Context { get; set; }

        public override void OnReceive(Context context, Intent intent)
        {
            Location location = intent.GetParcelableExtra(LocationUpdatesService.ExtraLocation) as Location;

            if (location != null)
            {
                Toast.MakeText(Context, Utils.GetLocationText(location), ToastLength.Short).Show();
            }
        }
    }
}