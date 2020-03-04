using System;
using System.ComponentModel;
using Tracked.Contexts;
using Tracked.Models;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace Tracked.Home {
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage {
        public MainPage(MainContext context) {
            InitializeComponent();
            BindingContext = new MainPageViewModel(context);
        }

        public MainPageViewModel ViewModel => BindingContext as MainPageViewModel;

        protected override void OnAppearing() {
            ViewModel.Refresh();

            base.OnAppearing();
        }

        private async void Add_Clicked(object sender, EventArgs e) {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            if (status != PermissionStatus.Granted) {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location)) {
                    await DisplayAlert("Location Required", "Location is required to record a ride", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                status = results[Permission.Location];
            }

            if (status == PermissionStatus.Granted) {
                await ViewModel.GoToCreateRide(Navigation);
            }
        }

        private async void Ride_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToReview(Navigation, e.Item as Ride);
        }
    }
}
