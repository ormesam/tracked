using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;

namespace MtbMate.Droid.Services {
    [Service]
    public class RideService : Service {
        private const string channelId = "default";
        private const string MAIN_ACTIVITY_ACTION = "Main_activity";
        public const string PUT_EXTRA = "has_service_been_started";
        private readonly IBinder binder;
        private const int notificationId = 12345678;
        private NotificationManager notificationManager;
        private Handler serviceHandler;

        public string Tag => "RideService";

        public RideService() {
            binder = new RideServiceBinder(this);
        }

        public override void OnCreate() {
            HandlerThread handlerThread = new HandlerThread(Tag);
            handlerThread.Start();
            serviceHandler = new Handler(handlerThread.Looper);
            notificationManager = (NotificationManager)GetSystemService(NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O) {
                string name = "RideForegroundService";
                NotificationChannel mChannel = new NotificationChannel(channelId, name, NotificationImportance.High);
                notificationManager.CreateNotificationChannel(mChannel);
            }
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId) {
            StartForeground(notificationId, GetNotification());

            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent) {
            return binder;
        }

        public override void OnDestroy() {
            serviceHandler.RemoveCallbacksAndMessages(null);
        }

        public void StartForegroundService() {
            StartService(new Intent(ApplicationContext, typeof(RideService)));
        }

        public void RemoveForegroundService() {
            StopSelf();
            StopForeground(true);
        }

        private Notification GetNotification() {
            return new NotificationCompat.Builder(this, channelId)
                .SetContentTitle("Mtb Mate")
                .SetContentText("Running...")
                .SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                .SetPriority((int)NotificationPriority.High)
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
                .Build();
        }

        private PendingIntent BuildIntentToShowMainActivity() {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.SetAction(MAIN_ACTIVITY_ACTION);
            notificationIntent.SetFlags(ActivityFlags.SingleTop);
            notificationIntent.PutExtra(PUT_EXTRA, true);

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);

            return pendingIntent;
        }
    }
}