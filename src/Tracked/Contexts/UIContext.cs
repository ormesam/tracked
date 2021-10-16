using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Screens.Feed;
using Tracked.Screens.Profile;
using Tracked.Screens.Record;
using Tracked.Screens.Rides;
using Tracked.Screens.Search;
using Tracked.Screens.Settings;
using Tracked.Screens.Settings.Modals;
using Tracked.Screens.Trails;
using Xamarin.Forms;

namespace Tracked.Contexts {
    public class UIContext {
        private readonly MainContext context;
        private bool isNavigating;

        public UIContext(MainContext context) {
            this.context = context;
        }

        private async Task ShowModal(Page page) {
            if (isNavigating) {
                return;
            }

            isNavigating = true;

            await App.RootPage.Navigation.PushModalAsync(page);

            isNavigating = false;
        }

        private async Task GoToScreenAsync(Page page) {
            if (isNavigating) {
                return;
            }

            isNavigating = true;

            await App.RootPage.Navigation.PushAsync(page);

            isNavigating = false;
        }

        private Task ReplaceScreenAsync(Page page) {
            if (isNavigating) {
                return Task.CompletedTask;
            }

            isNavigating = true;

            App.Current.MainPage = new NavigationPage(page);

            isNavigating = false;

            return Task.CompletedTask;
        }

        #region Root

        public async Task GoToFeedScreen() {
            await ReplaceScreenAsync(new FeedScreen(context));
        }

        public async Task GoToSearchScreenAsync() {
            await ReplaceScreenAsync(new SearchScreen(context));
        }

        public async Task GoToExploreTrailsScreenAsync() {
            await ReplaceScreenAsync(new ExploreTrailsScreen(context));
        }

        #endregion

        public async Task GoToRecordScreenAsync() {
            await GoToScreenAsync(new RecordScreen(context));
        }

        public async Task GoToRideReviewScreenAsync(int id) {
            RideReviewScreenViewModel viewModel = new RideReviewScreenViewModel(context);
            if (await viewModel.Load(id)) {
                await GoToScreenAsync(new RideReviewScreen(viewModel));
            }
        }

        public async Task GoToMapScreenAsync(RideDto ride) {
            await GoToScreenAsync(new MapScreen(context, ride));
        }

        public async Task GoToCreateTrailScreenAsync(RideDto ride = null) {
            await GoToScreenAsync(new CreateTrailScreen(context, ride));
        }

        public async Task GoToTrailScreenAsync(int trailId) {
            TrailScreenViewModel viewModel = new TrailScreenViewModel(context);
            if (await viewModel.Load(trailId)) {
                await GoToScreenAsync(new TrailScreen(viewModel));
            }
        }

        public async Task GoToSpeedAnalysisScreenAsync(IList<RideLocationDto> rideLocation) {
            await GoToScreenAsync(new SpeedAnalysisScreen(context, rideLocation));
        }

        public async Task GoToProfileScreenAsync() {
            var viewModel = new ProfileScreenViewModel(context);
            if (await viewModel.Load()) {
                await ReplaceScreenAsync(new ProfileScreen(viewModel));
            }
        }

        public async Task GoToSettingsScreenAsync() {
            await ReplaceScreenAsync(new SettingsScreen(context));
        }

        public async Task ShowJumpAboutModal() {
            await ShowModal(new JumpAboutModal(context));
        }

        public async Task GoToJumpAboutScreenAsync() {
            await GoToScreenAsync(new JumpAboutScreen(context));
        }

        public async Task<string> ShowPromptAsync(string title, string message, string defaultText) {
            return await App.Current.MainPage.DisplayPromptAsync(title, message, initialValue: defaultText);
        }
    }
}
