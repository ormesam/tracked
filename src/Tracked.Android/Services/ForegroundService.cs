using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;

namespace Tracked.Droid.Location {
    [Register("com.samorme.tracked.ForegroundService")]
    public class ForegroundService : Service {
        public static string Tag => typeof(ForegroundService).FullName;
        public const int ServiceRunningNotifID = 9000;
        private NotificationManager notificationManager;
        private const string channelId = "default";
        private const int notificationId = 12345678;

        public override IBinder OnBind(Intent intent) {
            return null;
        }

        public override void OnCreate() {
            notificationManager = (NotificationManager)GetSystemService(NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O) {
                NotificationChannel mChannel = new NotificationChannel(channelId, Tag, NotificationImportance.Default);
                notificationManager.CreateNotificationChannel(mChannel);
            }

            base.OnCreate();
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

        public override bool StopService(Intent name) {
            return base.StopService(name);
        }

        public override void OnDestroy() {
            notificationManager.Cancel(notificationId);

            base.OnDestroy();
        }
    }
}