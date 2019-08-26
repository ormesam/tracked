using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MtbMate.Home;
using MtbMate.Models;
using MtbMate.Screens.Bluetooth;
using MtbMate.Screens.Ride;
using MtbMate.Screens.Segments;
using MtbMate.Screens.Settings;
using MtbMate.Utilities;
using Xamarin.Forms;

namespace MtbMate.Contexts
{
    public class UIContext
    {
        private readonly MainContext context;
        private bool isNavigating;

        public UIContext(MainContext context) {
            this.context = context;
        }

        public void ShowInputDialog(string title, string defaultText, Action<string> onOk) {
            DependencyService.Get<IPromptUtility>().ShowInputDialog(title, defaultText, onOk);
        }

        public void ShowInputDialog(string title, string defaultText, Func<string, Task<string>> onOk) {
            DependencyService.Get<IPromptUtility>().ShowInputDialog(title, defaultText, onOk);
        }

        private async Task GoToScreenAsync(Page page) {
            if (isNavigating) {
                return;
            }

            isNavigating = true;

            if (page != null) {
                await App.RootPage.Detail.Navigation.PushAsync(page);
            }

            App.RootPage.IsPresented = false;

            isNavigating = false;
        }

        public async Task GoToRideScreenAsync(INavigation nav) {
            await nav.PushAsync(new RideScreen(context));
        }

        public async Task GoToReviewScreenAsync(INavigation nav, RideModel ride) {
            await nav.PushAsync(new ReviewScreen(context, ride));
        }

        public async Task GoToBluetoothScreenAsync() {
            await GoToScreenAsync(new BluetoothSetupScreen(context));
        }

        public async Task GoToSettingsScreenAsync() {
            await GoToScreenAsync(new SettingsScreen(context));
        }

        public async Task GoToMainPageAsync() {
            await GoToScreenAsync(new MainPage(context));
        }

        public async Task GoToMapScreenAsync(INavigation nav, RideModel ride) {
            await nav.PushAsync(new MapScreen(context, ride));
        }

        public async Task GoToMapScreenAsync(INavigation nav, string title, IList<LatLngModel> locations) {
            await nav.PushAsync(new MapScreen(context, title, locations));
        }

        public async Task GoToExploreSegmentsScreenAsync() {
            await GoToScreenAsync(new ExploreSegmentsScreen(context));
        }

        public async Task GoToCreateSegmentScreenAsync(INavigation nav) {
            await nav.PushAsync(new CreateSegmentScreen(context));
        }

        public async Task GoToSegmentScreenAsync(INavigation nav, SegmentModel segment) {
            await nav.PushAsync(new SegmentScreen(context, segment));
        }
    }
}
