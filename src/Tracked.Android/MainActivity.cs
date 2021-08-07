﻿using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using FFImageLoading.Forms.Platform;
using Microsoft.AppCenter.Crashes;
using Plugin.CurrentActivity;
using Tracked.Droid.Auth;
using Xamarin.Auth.Presenters.XamarinAndroid;

namespace Tracked.Droid {
    [Activity(Theme = "@style/MainTheme", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        public MainActivity() {
            TaskScheduler.UnobservedTaskException += (sender, args) => {
                Crashes.TrackError(args.Exception);
            };

            AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) => {
                Crashes.TrackError(args.Exception);
            };

            AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) => {
                if (e.ExceptionObject is Exception ex) {
                    Crashes.TrackError(ex);
                }
            };
        }

        protected override void OnCreate(Bundle bundle) {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            CachedImageRenderer.Init(enableFastRenderer: true);
            Xamarin.FormsMaps.Init(this, bundle);
            AuthenticationConfiguration.Init(this, bundle);
            GoogleClientManager.Init(this);
            XamEffects.Droid.Effects.Init();

            LoadApplication(new App());
        }

        protected override void OnStart() {
            base.OnStart();

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
            base.OnStop();

            Console.WriteLine();
            Console.WriteLine("STOPPED");
            Console.WriteLine();
        }

        protected override void OnDestroy() {
            base.OnDestroy();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);
            GoogleClientManager.OnAuthCompleted(requestCode, data);
        }
    }
}