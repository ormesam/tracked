using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MtbMate.Droid.Services;
using OxyPlot.Xamarin.Forms.Platform.Android;
using Plugin.CurrentActivity;

namespace MtbMate.Droid {
    [Activity(Label = "Mtb Mate", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        private readonly string[] Permissions =
         {
            Manifest.Permission.Bluetooth,
            Manifest.Permission.BluetoothAdmin,
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
            "android.permission.FOREGROUND_SERVICE",
            Manifest.Permission.AccessLocationExtraCommands,
            Manifest.Permission.AccessMockLocation,
            Manifest.Permission.AccessNetworkState,
            Manifest.Permission.AccessWifiState,
            Manifest.Permission.Internet,
        };

        public RideService Service { get; set; }
        public bool Bound { get; set; }
        public CustomServiceConnection ServiceConnection { get; set; }

        protected override void OnCreate(Bundle bundle) {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Essentials.Platform.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsGoogleMaps.Init(this, bundle);
            PlotViewRenderer.Init();

            ServiceConnection = new CustomServiceConnection { Activity = this };

            if (Intent.Action == RideService.MainActivityAction) {
                System.Diagnostics.Debug.WriteLine("Opened from ride notification");
            }

            CheckPermissions();

            LoadApplication(new App());
        }

        protected override void OnStart() {
            base.OnStart();

            BindService(new Intent(this, typeof(RideService)), ServiceConnection, Bind.AutoCreate);

            Console.WriteLine();
            Console.WriteLine("STARTED");
            Console.WriteLine();
        }

        protected override void OnResume() {
            base.OnResume();

            Console.WriteLine();
            Console.WriteLine("RESUMED");
            Console.WriteLine();
        }

        protected override void OnPause() {
            base.OnPause();

            Console.WriteLine();
            Console.WriteLine("PAUSED");
            Console.WriteLine();
        }

        protected override void OnStop() {
            if (Bound) {
                // Unbind from the service. This signals to the service that this activity is no longer
                // in the foreground, and the service can respond by promoting itself to a foreground
                // service.
                UnbindService(ServiceConnection);
                Bound = false;
            }

            base.OnStop();

            Console.WriteLine();
            Console.WriteLine("STOPPED");
            Console.WriteLine();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void CheckPermissions() {
            bool minimumPermissionsGranted = true;

            foreach (string permission in Permissions) {
                if (CheckSelfPermission(permission) != Permission.Granted) {
                    minimumPermissionsGranted = false;
                }
            }

            // If one of the minimum permissions aren't granted, we request them from the user
            if (!minimumPermissionsGranted) {
                RequestPermissions(Permissions, 0);
            }
        }
    }
}