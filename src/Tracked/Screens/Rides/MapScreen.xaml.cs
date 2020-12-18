using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Rides {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapScreen : ContentPage {
        public MapScreen(MainContext context, RideDto ride) {
            InitializeComponent();
            BindingContext = new MapScreenViewModel(context, ride);
        }

        public MapScreenViewModel ViewModel => BindingContext as MapScreenViewModel;

        protected override void OnAppearing() {
            base.OnAppearing();

            mapControl.CreateMap();
        }

        private void TrailAttempt_Tapped(object sender, ItemTappedEventArgs e) {
            ViewModel.HighlightTrail(e.Item as TrailAttemptDto);
        }
    }
}