using System;
using System.Collections.Generic;
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

            if (page != null) {
                await App.RootPage.Detail.Navigation.PushAsync(page);
            }

            App.RootPage.IsPresented = false;

            isNavigating = false;
        }

        public async Task GoToRecordScreenAsync(INavigation nav) {
            await nav.PushAsync(new RecordScreen(context));
        }

        public async Task GoToRideReviewScreenAsync(INavigation nav, IRide ride) {
            await nav.PushAsync(new RideReviewScreen(context, ride));
        }

        public async Task GoToAccelerometerReadingsScreenAsync(INavigation nav, IRide ride) {
            await nav.PushAsync(new AccelerometerReadingsScreen(context, ride));
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

        public async Task GoToAchievementOverviewScreenAsync() {
            await GoToScreenAsync(new AchievementOverviewScreen(context));
        }

        public async Task GoToAchievementScreenAsync(INavigation nav, IAchievement achievement) {
            await nav.PushAsync(new AchievementScreen(context, achievement));
        }

        public async Task GoToMapScreenAsync(INavigation nav, string title, IList<MapLocation> locations, bool showRideFeatures) {
            await nav.PushAsync(new MapScreen(context, title, locations, showRideFeatures));
        }

        public async Task GoToExploreSegmentsScreenAsync() {
            await GoToScreenAsync(new ExploreSegmentsScreen(context));
        }

        public async Task GoToCreateSegmentScreenAsync(INavigation nav) {
            await nav.PushAsync(new SelectRideScreen(context));
        }

        public async Task GoToCreateSegmentScreenAsync(INavigation nav, Ride ride) {
            await nav.PushAsync(new CreateSegmentScreen(context, ride));
        }

        public async Task GoToSegmentScreenAsync(INavigation nav, Segment segment) {
            await nav.PushAsync(new SegmentScreen(context, segment));
        }

        public async Task GoToSegmentAttemptScreenAsync(INavigation nav, SegmentAttempt attempt) {
            await GoToRideReviewScreenAsync(nav, attempt);
        }

        public async Task GoToSpeedAnalysisScreenAsync(INavigation nav, IRide ride) {
            await nav.PushAsync(new SpeedAnalysisScreen(context, ride));
        }
    }
}
