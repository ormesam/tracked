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
        private string channelId = "default";
        private readonly IBinder binder;
        private const int notificationId = 12345678;
        private bool changingConfiguration = false;
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

            CreateLocationRequest();
            GetLastLocation();

            HandlerThread handlerThread = new HandlerThread(Tag);
            handlerThread.Start();
            serviceHandler = new Handler(handlerThread.Looper);
            notificationManager = (NotificationManager)GetSystemService(NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                string name = "Location Updates ForegroundService";
                NotificationChannel mChannel = new NotificationChannel(channelId, name, NotificationImportance.High);
                notificationManager.CreateNotificationChannel(mChannel);
            }
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.NotSticky;
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            changingConfiguration = true;
        }

        public override IBinder OnBind(Intent intent)
        {
            StopForeground(true);

            changingConfiguration = false;

            return binder;
        }

        public override void OnRebind(Intent intent)
        {
            StopForeground(true);

            changingConfiguration = false;

            base.OnRebind(intent);
        }

        public override bool OnUnbind(Intent intent)
        {
            if (!changingConfiguration && Utils.RequestingLocationUpdates(this))
            {
                StartForeground(notificationId, GetNotification());
            }

            return true;
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
            try
            {
                fusedLocationClient.LastLocation.AddOnCompleteListener(new GetLastLocationOnCompleteListener { Service = this });
            }
            catch (SecurityException unlikely)
            {
                Log.Error(Tag, "Lost location permission." + unlikely);
            }
        }

        public void OnNewLocation(Location location)
        {
            Log.Info(Tag, "New location: " + location);

            this.Location = location;

            GeoUtility.Instance.UpdateLocation(location.Latitude, location.Longitude, location.Speed);
        }

        /**
	     * Sets the location request parameters.
	     */
        private void CreateLocationRequest()
        {
            locationRequest = new LocationRequest();
            locationRequest.SetInterval(3000);
            locationRequest.SetFastestInterval(2000);
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
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

    /**
	 * Class used for the client Binder.  Since this service runs in the same process as its
	 * clients, we don't need to deal with IPC.
	 */
    public class LocationUpdatesServiceBinder : Binder
    {
        readonly LocationUpdatesService service;

        public LocationUpdatesServiceBinder(LocationUpdatesService service)
        {
            this.service = service;
        }

        public LocationUpdatesService GetLocationUpdatesService()
        {
            return service;
        }
    }
}