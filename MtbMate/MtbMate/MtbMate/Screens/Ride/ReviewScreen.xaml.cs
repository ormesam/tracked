using System;
using System.Linq;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Essentials;
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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var firstLocation = ViewModel.Ride.Locations.FirstOrDefault();

            var pin = new Position(firstLocation.Latitude, firstLocation.Longitude);

            Map.MoveToRegion(MapSpan.FromCenterAndRadius(pin, Distance.FromMiles(0.5)));
        }

        public ReviewScreenViewModel ViewModel => BindingContext as ReviewScreenViewModel;

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
    }
}