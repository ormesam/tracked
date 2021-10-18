using System;
using System.ComponentModel;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms;

namespace Tracked.Screens.Feed {
    [DesignTimeVisible(true)]
    public partial class FeedScreen : ContentPage {
        public FeedScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new FeedScreenViewModel(context);
        }

        public FeedScreenViewModel ViewModel => BindingContext as FeedScreenViewModel;

        protected override async void OnAppearing() {
            base.OnAppearing();

            await ViewModel.Load();
        }

        private async void Add_Clicked(object sender, EventArgs e) {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();

            if (status != PermissionStatus.Granted) {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location)) {
                    await DisplayAlert("Location Required", "Location is required to record a ride", "OK");
                }

                status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
            }

            if (status == PermissionStatus.Granted) {
                await ViewModel.GoToCreateRide();
            }
        }

        private async void Ride_Tapped(object sender, EventArgs e) {
            var item = (sender as View).BindingContext as RideFeedDto;

            if (item == null) {
                return;
            }

            await ViewModel.GoToReview(item);
        }

        private async void Follow_Tapped(object sender, EventArgs e) {
            var item = (sender as View).BindingContext as FollowFeedDto;

            if (item == null) {
                return;
            }

            await ViewModel.GoToUserProfile(item);
        }
    }
}
