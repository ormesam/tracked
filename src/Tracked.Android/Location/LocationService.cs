using System;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Support.V4.App;
using Shared.Dtos;

namespace Tracked.Droid.Location {
    [Service]
    public class LocationService : Service, ILocationListener {
        private const string channelId = "default";
        public static string MainActivityAction = "OpenedFromLocationServiceNotification";
        private const int notificationId = 12345678;
        private NotificationManager notificationManager;
        private LocationManager locationManager;

        public static string Tag => typeof(LocationService).FullName;
        public IBinder Binder { get; private set; }

        public override void OnCreate() {
            base.OnCreate();

            notificationManager = (NotificationManager)GetSystemService(NotificationService);
            locationManager = (LocationManager)GetSystemService(LocationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O) {
                NotificationChannel mChannel = new NotificationChannel(channelId, Tag, NotificationImportance.Default);
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

        public void StartLocationUpdates() {
            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 1500, 0, this);
        }

        public void StopLocationUpdates() {
            locationManager.RemoveUpdates(this);
        }

        private Notification GetNotification() {
            return new NotificationCompat.Builder(this, channelId)
                .SetContentTitle("Tracked")
                .SetContentText("Running...")
                .SetContentIntent(BuildIntentToShowMainActivity())
                .SetOngoing(true)
                .SetPriority((int)NotificationPriority.High)
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
                .Build();
        }

        private PendingIntent BuildIntentToShowMainActivity() {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.SetAction(MainActivityAction);
            notificationIntent.SetFlags(ActivityFlags.SingleTop);

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);

            return pendingIntent;
        }

        public void OnLocationChanged(Android.Locations.Location location) {
            if (location.Accuracy > 20) {
                return;
            }

            var locationDto = new RideLocationDto {
                Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(location.Time).DateTime,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                AccuracyInMetres = location.Accuracy,
                Mph = location.Speed * 2.23694,
                Altitude = location.Altitude,
            };

            App.Current.MainContext.GeoUtility.AddLocation(locationDto);
        }

        public void OnProviderDisabled(string provider) {
            System.Diagnostics.Debug.WriteLine("provider disabled: " + provider);
        }

        public void OnProviderEnabled(string provider) {
            System.Diagnostics.Debug.WriteLine("provider enabled: " + provider);
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras) {
            System.Diagnostics.Debug.WriteLine("provider status changed: " + status);
        }

        public override void OnDestroy() {
            notificationManager.Cancel(notificationId);
            Binder = null;
            locationManager?.RemoveUpdates(this);
            locationManager?.Dispose();
            locationManager = null;

            base.OnDestroy();
        }
    }
}