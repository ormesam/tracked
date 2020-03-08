using Shared.Dtos;
using Tracked.Contexts;
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

        protected override async void OnAppearing() {
            base.OnAppearing();

            await ViewModel.Load();
        }

        private async void Add_Clicked(object sender, System.EventArgs e) {
            await ViewModel.AddSegment();
        }

        private async void Segment_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToSegment(e.Item as SegmentOverviewDto);
        }
    }
}