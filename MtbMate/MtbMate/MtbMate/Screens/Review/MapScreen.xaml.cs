using System.Collections.Generic;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapScreen : ContentPage {
        private MapScreen(MapScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;

            Map.RouteCoordinates = ViewModel.Locations;
            Map.ShowSpeed = ViewModel.ShowSpeed;
        }

        public MapScreen(MainContext context, string title, IList<Location> locations)
            : this(new MapScreenViewModel(context, title, locations)) {
        }

        public MapScreen(MainContext context, string title, IList<LatLng> locations)
            : this(new MapScreenViewModel(context, title, locations)) {
        }

        public MapScreenViewModel ViewModel => BindingContext as MapScreenViewModel;

        protected override void OnAppearing() {
            base.OnAppearing();

            Task.Run(() => {
                Map.GoToLocations(ViewModel.Locations);
            });
        }
    }
}