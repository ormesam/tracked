using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Ride {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapScreen : ContentPage {
        private MapScreen(MapScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;

            Map.RouteCoordinates = ViewModel.Locations;
            Map.ShowSpeed = ViewModel.ShowSpeed;
        }

        public MapScreen(MainContext context, string title, IList<LocationModel> locations)
            : this(new MapScreenViewModel(context, title, locations)) {
        }

        public MapScreen(MainContext context, string title, IList<LatLngModel> locations)
            : this(new MapScreenViewModel(context, title, locations)) {
        }

        public MapScreenViewModel ViewModel => BindingContext as MapScreenViewModel;

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
    }
}