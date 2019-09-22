using System;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSegmentScreen : ContentPage {
        public CreateSegmentScreen(MainContext context, Ride ride) {
            InitializeComponent();
            BindingContext = new CreateSegmentScreenViewModel(context, ride);

            Map.RouteCoordinates = PolyUtils.GetMapLocations(ride.Locations);
            Map.ShowSpeed = false;
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            Task.Run(() => {
                Map.GoToLocations(ViewModel.Ride.Locations);
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