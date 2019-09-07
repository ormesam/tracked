using System;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSegmentScreen : ContentPage {
        public CreateSegmentScreen(MainContext context, RideModel ride) {
            InitializeComponent();
            BindingContext = new CreateSegmentScreenViewModel(context, ride);

            Map.RouteCoordinates = ride.Locations;
            Map.ShowSpeed = false;
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            var firstLocation = ViewModel.Ride.Locations.Midpoint();

            var pin = new Position(firstLocation.LatLong.Latitude, firstLocation.LatLong.Longitude);

            Device.BeginInvokeOnMainThread(() => {
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(pin, Distance.FromMiles(0.25)));
            });
        }

        public CreateSegmentScreenViewModel ViewModel => BindingContext as CreateSegmentScreenViewModel;

        private void Save_Clicked(object sender, EventArgs e) {
            ViewModel.Save(Navigation);
        }

        private void Map_MapClicked(object sender, MapClickedEventArgs e) {
            ViewModel.AddPin(e.Position.Latitude, e.Position.Longitude);
        }
    }
}