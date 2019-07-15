using System;
using System.Collections.Generic;
using System.Diagnostics;
using Akavache;
using MtbMate.Accelerometer;
using MtbMate.Contexts;
using MtbMate.Home;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MtbMate
{
    public partial class App : Application
    {
        private readonly MainContext mainContext;

        public App()
        {
            InitializeComponent();

            mainContext = new MainContext();

            var navPage = new NavigationPage(new MainPage(mainContext));

            var bleToolbarItem = new ToolbarItem
            {
                IconImageSource = ImageSource.FromFile("bluetooth.png"),
            };

            bleToolbarItem.Clicked += async (s, e) =>
            {
                await mainContext.UI.GoToBluetoothScreen(navPage.Navigation);
            };

            navPage.ToolbarItems.Add(bleToolbarItem);

            MainPage = navPage;

            // initialise
            _ = BleAccelerometerUtility.Instance;

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
