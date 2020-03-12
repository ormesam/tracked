using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Achievements {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementScreen : ContentPage {
        public AchievementScreen(MainContext context, AchievementDto achievement) {
            InitializeComponent();
            BindingContext = new AchievementScreenViewModel(context, achievement);
        }

        public AchievementScreenViewModel ViewModel => BindingContext as AchievementScreenViewModel;

        private async void Ride_Tapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToRide(e.Item as RideOverviewDto);
        }
    }
}