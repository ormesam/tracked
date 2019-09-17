using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttemptScreen : ContentPage {
        public AttemptScreen(MainContext context, SegmentAttempt attempt) {
            InitializeComponent();
            BindingContext = new AttemptScreenViewModel(context, attempt);

            Map.RouteCoordinates = ViewModel.Locations;
            Map.ShowSpeed = true;
        }

        public AttemptScreenViewModel ViewModel => BindingContext as AttemptScreenViewModel;

        protected override void OnAppearing() {
            base.OnAppearing();

            Task.Run(() => {
                Map.GoToLocations(ViewModel.Locations);
            });
        }

        private async void Map_MapClicked(object sender, MapClickedEventArgs e) {
            await ViewModel.GoToMapScreen(Navigation);
        }
    }
}