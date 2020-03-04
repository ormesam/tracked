using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracked.Achievements;
using Tracked.Dependancies;
using Tracked.Home;
using Tracked.Models;
using Tracked.Screens.Achievements;
using Tracked.Screens.Bluetooth;
using Tracked.Screens.Review;
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

        public async Task GoToRideReviewScreenAsync(IRide ride) {
            await GoToScreenAsync(new RideReviewScreen(context, ride));
        }

        public async Task GoToAccelerometerReadingsScreenAsync(IRide ride) {
            await GoToScreenAsync(new AccelerometerReadingsScreen(context, ride));
        }

        public async Task GoToAchievementScreenAsync(IAchievement achievement) {
            await GoToScreenAsync(new AchievementScreen(context, achievement));
        }

        public async Task GoToMapScreenAsync(string title, IList<MapLocation> locations, bool showRideFeatures) {
            await GoToScreenAsync(new MapScreen(context, title, locations, showRideFeatures));
        }

        public async Task GoToCreateSegmentScreenAsync() {
            await GoToScreenAsync(new SelectRideScreen(context));
        }

        public async Task GoToCreateSegmentScreenAsync(Ride ride) {
            await GoToScreenAsync(new CreateSegmentScreen(context, ride));
        }

        public async Task GoToSegmentScreenAsync(Segment segment) {
            await GoToScreenAsync(new SegmentScreen(context, segment));
        }

        public async Task GoToSegmentAttemptScreenAsync(SegmentAttempt attempt) {
            await GoToRideReviewScreenAsync(attempt);
        }

        public async Task GoToSpeedAnalysisScreenAsync(IRide ride) {
            await GoToScreenAsync(new SpeedAnalysisScreen(context, ride));
        }
    }
}
