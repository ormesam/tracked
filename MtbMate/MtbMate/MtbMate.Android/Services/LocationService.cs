using System;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Gms.Tasks;
using Android.Locations;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Java.Lang;
using MtbMate.Utilities;
using Task = Android.Gms.Tasks.Task;

namespace MtbMate.Droid.Services
{
    /**
	 * A bound and started service that is promoted to a foreground service when location updates have
	 * been requested and all clients unbind.
	 *
	 * For apps running in the background on "O" devices, location is computed only once every 10
	 * minutes and delivered batched every 30 minutes. This restriction applies even to apps
	 * targeting "N" or lower which are run on "O" devices.
	 *
	 * This sample show how to use a long-running service for location updates. When an activity is
	 * bound to this service, frequent location updates are permitted. When the activity is removed
	 * from the foreground, the service promotes itself to a foreground service, and location updates
	 * continue. When the activity comes back to the foreground, the foreground service stops, and the
	 * notification assocaited with that service is removed.
	 */
    [Service(Label = "LocationUpdatesService")]
    [IntentFilter(new string[] { "com.xamarin.LocUpdFgService.LocationUpdatesService" })]
    public class LocationUpdatesService : Service
    {
        private const string LocationPackageName = "com.xamarin.LocUpdFgService";
        public string Tag = "LocationUpdatesService";
        private string channelId = "default";
        public const string actionBroadcast = LocationPackageName + ".broadcast";
        public const string extraLocation = LocationPackageName + ".location";
        private IBinder binder;
        private const int notificationId = 12345678;
        private bool changingConfiguration = false;
        private NotificationManager NotificationManager;
        private LocationRequest locationRequest;
        private FusedLocationProviderClient fusedLocationClient;
        private LocationCallback locationCallback;
        private Handler serviceHandler;
        public Location location;

        public LocationUpdatesService()
        {
            binder = new LocationUpdatesServiceBinder(this);
        }

        class LocationCallbackImpl : LocationCallback
        {
            public LocationUpdatesService Service { get; set; }
            public override void OnLocationResult(LocationResult result)
            {
                base.OnLocationResult(result);
                Service.OnNewLocation(result.LastLocation);
            }
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
            NotificationManager = (NotificationManager)GetSystemService(NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                string name = "Location Updates ForegroundService";
                NotificationChannel mChannel = new NotificationChannel(channelId, name, NotificationImportance.High);
                NotificationManager.CreateNotificationChannel(mChannel);
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
            Log.Info(Tag, "Requesting location updates");

            Utils.SetRequestingLocationUpdates(this, true);

            StartService(new Intent(ApplicationContext, typeof(LocationUpdatesService)));

            fusedLocationClient.RequestLocationUpdates(locationRequest, locationCallback, Looper.MyLooper());
        }

        public void RemoveLocationUpdates()
        {
            Log.Info(Tag, "Removing location updates");

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

            this.location = location;

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