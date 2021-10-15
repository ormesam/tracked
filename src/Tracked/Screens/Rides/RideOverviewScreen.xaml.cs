using System;
using System.ComponentModel;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms;

namespace Tracked.Screens.Rides {
    [DesignTimeVisible(true)]
    public partial class RideOverviewScreen : ContentPage {
        public RideOverviewScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new RideOverviewScreenViewModel(context);
        }

        public RideOverviewScreenViewModel ViewModel => BindingContext as RideOverviewScreenViewModel;

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

        private void Follow_Tapped(object sender, EventArgs e) {
            // TODO: go to users page
        }
    }
}
