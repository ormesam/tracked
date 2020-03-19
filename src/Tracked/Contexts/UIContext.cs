using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Dependancies;
using Tracked.Home;
using Tracked.Models;
using Tracked.Screens.Achievements;
using Tracked.Screens.Bluetooth;
using Tracked.Screens.Review;
using Tracked.Screens.SegmentAttempt;
using Tracked.Screens.Segments;
using Tracked.Screens.Settings;
using Xamarin.Forms;

namespace Tracked.Contexts {
    public class UIContext {
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

        public async Task GoToAchievementOverviewScreenAsync() {
            await GoToSideBarItemAsync(new AchievementOverviewScreen(context));
        }

        public async Task GoToExploreSegmentsScreenAsync() {
            await GoToSideBarItemAsync(new ExploreSegmentsScreen(context));
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

        public async Task GoToAchievementScreenAsync(AchievementDto achievement) {
            await GoToScreenAsync(new AchievementScreen(context, achievement));
        }

        public async Task GoToMapScreenAsync(string title, IList<MapLocation> locations, bool showRideFeatures) {
            await GoToScreenAsync(new MapScreen(context, title, locations, showRideFeatures));
        }

        public async Task GoToCreateSegmentScreenAsync(RideDto ride = null) {
            await GoToScreenAsync(new CreateSegmentScreen(context, ride));
        }

        public async Task GoToSegmentScreenAsync(int segmentId) {
            SegmentScreenViewModel viewModel = new SegmentScreenViewModel(context);
            await viewModel.Load(segmentId);
            await GoToScreenAsync(new SegmentScreen(viewModel));
        }

        public async Task GoToSegmentAttemptScreenAsync(int segmentAttemptId) {
            SegmentAttemptScreenViewModel viewModel = new SegmentAttemptScreenViewModel(context);
            await viewModel.Load(segmentAttemptId);
            await GoToScreenAsync(new SegmentAttemptScreen(viewModel));
        }

        public async Task GoToSpeedAnalysisScreenAsync(IList<RideLocationDto> rideLocation) {
            await GoToScreenAsync(new SpeedAnalysisScreen(context, rideLocation));
        }
    }
}
