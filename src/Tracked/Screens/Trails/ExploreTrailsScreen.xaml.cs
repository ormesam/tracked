using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Trails {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExploreTrailsScreen : ContentPage {
        public ExploreTrailsScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new ExploreTrailsScreenViewModel(context);
        }

        public ExploreTrailsScreenViewModel ViewModel => BindingContext as ExploreTrailsScreenViewModel;

        protected override async void OnAppearing() {
            base.OnAppearing();

            await ViewModel.Load();
        }

        private async void Add_Clicked(object sender, System.EventArgs e) {
            await ViewModel.AddTrail();
        }

        private async void Trail_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToTrail(e.Item as TrailOverviewDto);
        }
    }
}