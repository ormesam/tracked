
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;

namespace Tracked.Droid.Location {
    [Service]
    public class LocationService : Service {
        private const string channelId = "default";
        private const int notificationId = 12345678;
        private NotificationManager notificationManager;
        private LocationUpdater locationUpdater;

        public static string Tag => typeof(LocationService).FullName;
        public IBinder Binder { get; private set; }

        public override void OnCreate() {
            base.OnCreate();

            notificationManager = (NotificationManager)GetSystemService(NotificationService);
            locationUpdater = new LocationUpdater();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O) {
                string name = "LocationForegroundService";
                NotificationChannel mChannel = new NotificationChannel(channelId, name, NotificationImportance.High);
                notificationManager.CreateNotificationChannel(mChannel);
            }
        }

        public override IBinder OnBind(Intent intent) {
            Binder = new LocationBinder(this);
            return Binder;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId) {
            StartForeground(notificationId, GetNotification());

            return StartCommandResult.Sticky;
        }

        private Notification GetNotification() {
            return new NotificationCompat.Builder(this, channelId)
                .SetContentTitle("Tracked")
                .SetContentText("Running...")
                .SetOngoing(true)
                .SetPriority((int)NotificationPriority.High)
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
                .Build();
        }

        public override void OnDestroy() {
            notificationManager.Cancel(notificationId);
            Binder = null;
            locationUpdater?.Dispose();
            locationUpdater = null;

            base.OnDestroy();
        }
    }
}