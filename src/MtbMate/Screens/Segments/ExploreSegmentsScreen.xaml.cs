using Tracked.Contexts;
using Tracked.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Settings {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExploreSegmentsScreen : ContentPage {
        public ExploreSegmentsScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new ExploreSegmentsScreenViewModel(context);
        }

        public ExploreSegmentsScreenViewModel ViewModel => BindingContext as ExploreSegmentsScreenViewModel;

        protected override void OnAppearing() {
            base.OnAppearing();

            ViewModel.Refresh();
        }

        private async void Add_Clicked(object sender, System.EventArgs e) {
            await ViewModel.AddSegment(Navigation);
        }

        private async void Segments_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToSegment(Navigation, e.Item as Segment);
        }
    }
}