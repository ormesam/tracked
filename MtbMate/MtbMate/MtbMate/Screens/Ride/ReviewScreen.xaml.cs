using System;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Ride
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewScreen : ContentPage
    {
        public ReviewScreen(MainContext context, RideModel ride)
        {
            InitializeComponent();
            BindingContext = new ReviewScreenViewModel(context, ride);

            Map.RouteCoordinates = ViewModel.Locations;
        }

        public ReviewScreenViewModel ViewModel => BindingContext as ReviewScreenViewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(() =>
            {
                var firstLocation = ViewModel.Ride.Locations.FirstOrDefault();

                var pin = new Position(firstLocation.LatLong.Latitude, firstLocation.LatLong.Longitude);

                Device.BeginInvokeOnMainThread(() =>
                {
                    Map.MoveToRegion(MapSpan.FromCenterAndRadius(pin, Distance.FromMiles(0.25)));
                });
            });
        }

        private async void Export_Clicked(object sender, EventArgs e)
        {
            await ViewModel.Export();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await ViewModel.Delete();

            await Navigation.PopToRootAsync();
        }

        private void Name_Tapped(object sender, EventArgs e)
        {
            ViewModel.ChangeName();
        }

        private async void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            await ViewModel.GoToMapScreen(Navigation);
        }

        private async void ExportLocation_Clicked(object sender, EventArgs e)
        {
            await ViewModel.ExportLocation();
        }
    }
}