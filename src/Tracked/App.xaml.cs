using System;
using Akavache;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Shared;
using Tracked.Contexts;
using Tracked.Screens.Login;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tracked {
    public partial class App : Application {
        public MainContext MainContext { get; }
        public static new App Current => (App)Application.Current;
        public static NavigationPage RootPage => (NavigationPage)Current.MainPage;

        public App() {
            InitializeComponent();

            VersionTracking.Track();

            MainContext = new MainContext();
            MainPage = new LoginScreen(MainContext, !VersionTracking.IsFirstLaunchEver);
        }

        protected override void OnStart() {
            AppCenter.Start($"android={Constants.AppCenterKey};", typeof(Analytics), typeof(Crashes));

            BlobCache.EnsureInitialized();
            BlobCache.ApplicationName = Constants.AppName;
            BlobCache.ForcedDateTimeKind = DateTimeKind.Utc;
        }

        protected override void OnSleep() {
            MainContext.Storage.Storage.Flush();
        }
    }
}
