using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.CurrentActivity;
using Android;
using MtbMate.Droid.Services;
using Android.Content;
using Android.Support.V4.Content;
using Android.Preferences;
using System;

namespace MtbMate.Droid
{
    [Activity(Label = "Mtb Mate", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ISharedPreferencesOnSharedPreferenceChangeListener
    {
        private readonly string[] Permissions =
         {
            Manifest.Permission.Bluetooth,
            Manifest.Permission.BluetoothAdmin,
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
            "android.permission.FOREGROUND_SERVICE",
        };

        public LocationUpdatesService Service { get; set; }
        public bool Bound { get; set; }
        public CustomServiceConnection ServiceConnection { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            ServiceConnection = new CustomServiceConnection { Activity = this };

            CheckPermissions();

            LoadApplication(new App());
        }

        protected override void OnStart()
        {
            base.OnStart();

            BindService(new Intent(this, typeof(LocationUpdatesService)), ServiceConnection, Bind.AutoCreate);

            Console.WriteLine();
            Console.WriteLine("STARTED");
            Console.WriteLine();
        }

        protected override void OnResume()
        {
            base.OnResume();

            Console.WriteLine();
            Console.WriteLine("RESUMED");
            Console.WriteLine();
        }

        protected override void OnPause()
        {
            base.OnPause();

            Console.WriteLine();
            Console.WriteLine("PAUSED");
            Console.WriteLine();
        }

        protected override void OnStop()
        {
            if (Bound)
            {
                // Unbind from the service. This signals to the service that this activity is no longer
                // in the foreground, and the service can respond by promoting itself to a foreground
                // service.
                UnbindService(ServiceConnection);
                Bound = false;
            }

            PreferenceManager.GetDefaultSharedPreferences(this).UnregisterOnSharedPreferenceChangeListener(this);

            base.OnStop();

            Console.WriteLine();
            Console.WriteLine("STOPPED");
            Console.WriteLine();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void CheckPermissions()
        {
            bool minimumPermissionsGranted = true;

            foreach (string permission in Permissions)
            {
                if (CheckSelfPermission(permission) != Permission.Granted)
                {
                    minimumPermissionsGranted = false;
                }
            }

            // If one of the minimum permissions aren't granted, we request them from the user
            if (!minimumPermissionsGranted)
            {
                RequestPermissions(Permissions, 0);
            }
        }

        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            // Update the buttons state depending on whether location updates are being requested.
            if (key.Equals(Utils.KeyRequestingLocationUpdates))
            {
                Console.WriteLine("OnSharedPreferenceChanged called");
                // SetButtonsState(sharedPreferences.GetBoolean(Utils.KeyRequestingLocationUpdates, false));
            }
        }
    }
}