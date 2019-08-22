using MtbMate.Contexts;
using MtbMate.Models;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSegmentScreen : ContentPage
    {
        public CreateSegmentScreen(MainContext context)
        {
            InitializeComponent();
            BindingContext = new CreateSegmentScreenViewModel(context);
        }

        public CreateSegmentScreenViewModel ViewModel => BindingContext as CreateSegmentScreenViewModel;

        private void Ride_Tapped(object sender, ItemTappedEventArgs e)
        {
            ViewModel.SelectedRide = e.Item as RideModel;

            var firstLocation = ViewModel.SelectedRide.Locations.FirstOrDefault();

            var pin = new Position(firstLocation.LatLong.Latitude, firstLocation.LatLong.Longitude);

            Device.BeginInvokeOnMainThread(() =>
            {
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(pin, Distance.FromMiles(0.25)));
            });
        }

        private void Save_Clicked(object sender, EventArgs e) {
            ViewModel.Save(Navigation);
        }
    }
}