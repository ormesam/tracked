using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Java.Lang;
using MtbMate.Utilities;

namespace MtbMate.Droid.Services
{
    [Service(Label = "LocationUpdatesService")]
    [IntentFilter(new string[] { "com.xamarin.LocUpdFgService.LocationUpdatesService" })]
    public class LocationUpdatesService : Service
    {
        private const string channelId = "default";
        private readonly IBinder binder;
        private const int notificationId = 12345678;
        private NotificationManager notificationManager;
        private LocationRequest locationRequest;
        private FusedLocationProviderClient fusedLocationClient;
        private LocationCallback locationCallback;
        private Handler serviceHandler;

        public Location Location { get; set; }
        public string Tag => "LocationUpdatesService";

        public LocationUpdatesService()
        {
            binder = new LocationUpdatesServiceBinder(this);
        }

        public override void OnCreate()
        {
            fusedLocationClient = LocationServices.GetFusedLocationProviderClient(this);
            locationCallback = new LocationCallbackImpl { Service = this };

            locationRequest = new LocationRequest();
            locationRequest.SetInterval(3000);
            locationRequest.SetFastestInterval(2000);
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);

            GetLastLocation();

            HandlerThread handlerThread = new HandlerThread(Tag);
            handlerThread.Start();
            serviceHandler = new Handler(handlerThread.Looper);
            notificationManager = (NotificationManager)GetSystemService(NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                string name = "LocationForegroundService";
                NotificationChannel mChannel = new NotificationChannel(channelId, name, NotificationImportance.High);
                notificationManager.CreateNotificationChannel(mChannel);
            }
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            StartForeground(notificationId, GetNotification());

            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return binder;
        }

        public override void OnDestroy()
        {
            serviceHandler.RemoveCallbacksAndMessages(null);
        }

        public void RequestLocationUpdates()
        {
            Utils.SetRequestingLocationUpdates(this, true);

            StartService(new Intent(ApplicationContext, typeof(LocationUpdatesService)));

            fusedLocationClient.RequestLocationUpdates(locationRequest, locationCallback, Looper.MyLooper());
        }

        public void RemoveLocationUpdates()
        {
            fusedLocationClient.RemoveLocationUpdates(locationCallback);

            Utils.SetRequestingLocationUpdates(this, false);

            StopSelf();
        }

        private Notification GetNotification()
        {
            return new NotificationCompat.Builder(this, channelId)
                .SetContentTitle("Mtb Mate")
                .SetContentText("Running...")
                .SetOngoing(true)
                .SetPriority((int)NotificationPriority.High)
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
                .Build();
        }

        private void GetLastLocation()
        {
            fusedLocationClient.LastLocation.AddOnCompleteListener(new GetLastLocationOnCompleteListener { Service = this });
        }

        public void OnNewLocation(Location location)
        {
            Log.Info(Tag, "New location: " + location);

            this.Location = location;

            GeoUtility.Instance.UpdateLocation(location.Latitude, location.Longitude, location.Speed);
        }

        private class LocationCallbackImpl : LocationCallback
        {
            public LocationUpdatesService Service { get; set; }

            public override void OnLocationResult(LocationResult result)
            {
                base.OnLocationResult(result);

                Service.OnNewLocation(result.LastLocation);
            }
        }
    }
}