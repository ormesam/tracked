using MtbMate.Contexts;
using MtbMate.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SegmentScreen : ContentPage
    {
        public SegmentScreen(MainContext context, SegmentModel segment) {
            InitializeComponent();
            BindingContext = new SegmentScreenViewModel(context, segment);

            Map.RouteCoordinates = ViewModel.Locations;
        }

        public SegmentScreenViewModel ViewModel => BindingContext as SegmentScreenViewModel;

        protected override void OnAppearing() {
            base.OnAppearing();

            Task.Run(() => {
                var firstLocation = ViewModel.Locations.FirstOrDefault();

                var pin = new Position(firstLocation.LatLong.Latitude, firstLocation.LatLong.Longitude);

                Device.BeginInvokeOnMainThread(() => {
                    Map.MoveToRegion(MapSpan.FromCenterAndRadius(pin, Distance.FromMiles(0.25)));
                });
            });
        }

        private void Name_Tapped(object sender, EventArgs e) {
            ViewModel.ChangeName();
        }

        private async void Map_MapClicked(object sender, MapClickedEventArgs e) {
            await ViewModel.GoToMapScreen(Navigation);
        }

        private void Analyse_Clicked(object sender, EventArgs e) {
            ViewModel.AnalyseExistingRides();
        }
    }
}