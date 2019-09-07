using MtbMate.Contexts;
using MtbMate.Models;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttemptScreen : ContentPage {
        public AttemptScreen(MainContext context, SegmentAttemptModel attempt) {
            InitializeComponent();
            BindingContext = new AttemptScreenViewModel(context, attempt);

            Map.RouteCoordinates = ViewModel.Locations;
            Map.ShowSpeed = true;
        }

        public AttemptScreenViewModel ViewModel => BindingContext as AttemptScreenViewModel;

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

        private async void Map_MapClicked(object sender, MapClickedEventArgs e) {
            await ViewModel.GoToMapScreen(Navigation);
        }
    }
}