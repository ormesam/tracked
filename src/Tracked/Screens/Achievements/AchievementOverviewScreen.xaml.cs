using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementOverviewScreen : ContentPage {
        public AchievementOverviewScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new AchievementOverviewScreenViewModel(context);
        }

        public AchievementOverviewScreenViewModel ViewModel => BindingContext as AchievementOverviewScreenViewModel;

        protected override async void OnAppearing() {
            base.OnAppearing();

            await ViewModel.Load();
        }

        private async void Achievements_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToAchievement(e.Item as AchievementDto);
        }
    }
}