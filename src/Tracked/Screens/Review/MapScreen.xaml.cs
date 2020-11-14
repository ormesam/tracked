using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapScreen : ContentPage {
        public MapScreen(MainContext context, RideDto ride) {
            InitializeComponent();
            BindingContext = new MapScreenViewModel(context, ride);
        }

        public MapScreenViewModel ViewModel => BindingContext as MapScreenViewModel;

        private void SegmentAttempt_Tapped(object sender, ItemTappedEventArgs e) {
            ViewModel.HighlightSegment(e.Item as SegmentAttemptDto);
        }
    }
}