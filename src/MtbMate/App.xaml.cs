using System;
using Akavache;
using Tracked.Accelerometer;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Screens.Master;
using Xamarin.Essentials;
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

            MainPage = new MasterScreen(mainContext);

            ExperimentalFeatures.Enable("ShareFileRequest_Experimental");
        }

        protected override void OnStart() {
            // Handle when your app starts
            BlobCache.EnsureInitialized();
#if DEBUG
            BlobCache.ApplicationName = "Mtb Mate Dev";
#else
            BlobCache.ApplicationName = "Mtb Mate";
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
