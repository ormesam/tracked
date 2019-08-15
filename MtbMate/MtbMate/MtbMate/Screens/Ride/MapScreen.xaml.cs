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
    public partial class MapScreen : ContentPage
    {
        public MapScreen(MainContext context, RideModel ride)
        {
            InitializeComponent();
            BindingContext = new MapScreenViewModel(context, ride);

            Map.RouteCoordinates = ViewModel.Ride.GetLocationSteps();
        }

        public MapScreenViewModel ViewModel => BindingContext as MapScreenViewModel;

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
    }
}