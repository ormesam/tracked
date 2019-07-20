using Android.Content;
using Android.Preferences;

namespace MtbMate.Droid.Services
{
    public class Utils
    {
        public const string KeyRequestingLocationUpdates = "requesting_locaction_updates";

        /**
	     * Returns true if requesting location updates, otherwise returns false.
	     *
	     * @param context The {@link Context}.
	     */
        public static bool RequestingLocationUpdates(Context context)
        {
            return PreferenceManager.GetDefaultSharedPreferences(context)
                    .GetBoolean(KeyRequestingLocationUpdates, false);
        }

        /**
	     * Stores the location updates state in SharedPreferences.
	     * @param requestingLocationUpdates The location updates state.
	     */
        public static void SetRequestingLocationUpdates(Context context, bool requestingLocationUpdates)
        {
            PreferenceManager.GetDefaultSharedPreferences(context)
                .Edit()
                .PutBoolean(KeyRequestingLocationUpdates, requestingLocationUpdates)
                .Apply();
        }
    }
}