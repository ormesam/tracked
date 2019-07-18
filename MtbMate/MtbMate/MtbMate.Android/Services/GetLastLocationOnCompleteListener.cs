using Android.Gms.Tasks;
using Android.Locations;
using Android.Util;
using Java.Lang;
using Task = Android.Gms.Tasks.Task;

namespace MtbMate.Droid.Services
{
    public class GetLastLocationOnCompleteListener : Object, IOnCompleteListener
    {
        public LocationService Service { get; set; }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful && task.Result != null)
            {
                Service.Location = (Location)task.Result;
            }
            else
            {
                Log.Warn(Service.Tag, "Failed to get location.");
            }
        }
    }
}