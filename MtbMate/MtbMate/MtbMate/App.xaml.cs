using System;
using Akavache;
using MtbMate.Contexts;
using MtbMate.Home;
using Xamarin.Forms;

namespace MtbMate
{
    public partial class App : Application
    {
        private MainContext mainContext;

        public App()
        {
            InitializeComponent();

            mainContext = new MainContext();

            MainPage = new NavigationPage(new MainPage(mainContext));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            BlobCache.ApplicationName = "Mtb Mate";
            BlobCache.ForcedDateTimeKind = DateTimeKind.Utc;
        }

        protected override void OnSleep()
        {
            BlobCache.Shutdown().Wait();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
