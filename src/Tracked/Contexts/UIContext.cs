using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Home;
using Tracked.Screens.Bluetooth;
using Tracked.Screens.Profile;
using Tracked.Screens.Record;
using Tracked.Screens.Rides;
using Tracked.Screens.Settings;
using Tracked.Screens.Trails;
using Xamarin.Forms;

namespace Tracked.Contexts {
    public class UIContext {
        private readonly MainContext context;
        private bool isNavigating;

        public UIContext(MainContext context) {
            this.context = context;
        }

        private async Task GoToScreenAsync(Page page) {
            if (isNavigating) {
                return;
            }

            isNavigating = true;

            await App.RootPage.Detail.Navigation.PushAsync(page);

            isNavigating = false;
        }

        private async Task GoToSideBarItemAsync(Page page) {
            if (isNavigating) {
                return;
            }

            isNavigating = true;

            if (page != null) {
                var currentPage = App.RootPage.Detail.Navigation.NavigationStack.FirstOrDefault();

                await App.RootPage.Detail.Navigation.PushAsync(page);

                if (currentPage != null) {
                    App.RootPage.Detail.Navigation.RemovePage(currentPage);
                }
            }

            App.RootPage.IsPresented = false;

            isNavigating = false;
        }

        #region Sidebar

        public async Task GoToMainPageAsync() {
            await GoToSideBarItemAsync(new MainPage(context));
        }

        public async Task GoToBluetoothScreenAsync() {
            await GoToSideBarItemAsync(new BluetoothSetupScreen(context));
        }

        public async Task GoToSettingsScreenAsync() {
            await GoToSideBarItemAsync(new SettingsScreen(context));
        }

        public async Task GoToExploreTrailsScreenAsync() {
            await GoToSideBarItemAsync(new ExploreTrailsScreen(context));
        }

        #endregion

        public async Task GoToRecordScreenAsync() {
            await GoToScreenAsync(new RecordScreen(context));
        }

        public async Task GoToRideReviewScreenAsync(int id) {
            RideReviewScreenViewModel viewModel = new RideReviewScreenViewModel(context);
            await viewModel.Load(id);
            await GoToScreenAsync(new RideReviewScreen(viewModel));
        }

        public async Task GoToMapScreenAsync(RideDto ride) {
            await GoToScreenAsync(new MapScreen(context, ride));
        }

        public async Task GoToCreateTrailScreenAsync(RideDto ride = null) {
            await GoToScreenAsync(new CreateTrailScreen(context, ride));
        }

        public async Task GoToTrailScreenAsync(int trailId) {
            TrailScreenViewModel viewModel = new TrailScreenViewModel(context);
            await viewModel.Load(trailId);
            await GoToScreenAsync(new TrailScreen(viewModel));
        }

        public async Task GoToSpeedAnalysisScreenAsync(IList<RideLocationDto> rideLocation) {
            await GoToScreenAsync(new SpeedAnalysisScreen(context, rideLocation));
        }

        public async Task GoToProfileScreenAsync() {
            var viewModel = new ProfileScreenViewModel(context);
            await viewModel.Load();
            await GoToScreenAsync(new ProfileScreen(viewModel));
        }

        public async Task<string> ShowPromptAsync(string title, string message, string defaultText) {
            return await App.Current.MainPage.DisplayPromptAsync(title, message, initialValue: defaultText);
        }
    }
}
