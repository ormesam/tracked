using System;
using Akavache;
using Tracked.Accelerometer;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Screens.Login;
using Tracked.Screens.Master;
using Xamarin.Forms;

namespace Tracked {
    public partial class App : Application {
        private readonly MainContext mainContext;
        public static new App Current => (App)Application.Current;
        public static MasterScreen RootPage => Current.MainPage as MasterScreen;

        public App() {
            InitializeComponent();

            mainContext = new MainContext();

            // initialise
            _ = AccelerometerUtility.Instance;
            Model.Instance.Init(mainContext);

            if (mainContext.Security.IsLoggedIn) {
                MainPage = new MasterScreen(mainContext);
            } else {
                MainPage = new LoginScreen(mainContext);
            }
        }

        protected override void OnStart() {
            // Handle when your app starts
            BlobCache.EnsureInitialized();
#if DEBUG
            BlobCache.ApplicationName = "Tracked Dev";
#else
            BlobCache.ApplicationName = "Tracked";
#endif
            BlobCache.ForcedDateTimeKind = DateTimeKind.Utc;
        }

        protected override void OnSleep() {
            mainContext.Storage.Storage.Flush();
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }
}
