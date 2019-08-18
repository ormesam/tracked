using System;
using System.Collections.Generic;
using System.Diagnostics;
using Akavache;
using MtbMate.Accelerometer;
using MtbMate.Contexts;
using MtbMate.Home;
using MtbMate.Screens.Master;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MtbMate
{
    public partial class App : Application
    {
        private readonly MainContext mainContext;
        public static new App Current => (App)Application.Current;
        public static MasterScreen RootPage => Current.MainPage as MasterScreen;

        public App()
        {
            InitializeComponent();

            mainContext = new MainContext();

            MainPage = new MasterScreen(mainContext);

            // initialise
            _ = AccelerometerUtility.Instance;

            ExperimentalFeatures.Enable("ShareFileRequest_Experimental");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            BlobCache.EnsureInitialized();
            BlobCache.ApplicationName = "Mtb Mate";
            BlobCache.ForcedDateTimeKind = DateTimeKind.Utc;

            Debug.WriteLine(BlobCache.LocalMachine.GetType());
        }

        protected override void OnSleep()
        {
            mainContext.Storage.Storage.Flush();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
