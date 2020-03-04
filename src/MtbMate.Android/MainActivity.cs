﻿using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MtbMate.Droid.Services;
using OxyPlot.Xamarin.Forms.Platform.Android;
using Plugin.CurrentActivity;
using Xamarin.Auth;
using Xamarin.Auth.Presenters.XamarinAndroid;

namespace MtbMate.Droid {
    [Activity(Label = "Mtb Mate Dev", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
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
            AuthenticationConfiguration.Init(this, bundle);
            CustomTabsConfiguration.CustomTabsClosingMessage = null;

            ServiceConnection = new CustomServiceConnection { Activity = this };

            if (Intent.Action == RideService.MainActivityAction) {
                System.Diagnostics.Debug.WriteLine("Opened from ride notification");
            }

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
    }
}