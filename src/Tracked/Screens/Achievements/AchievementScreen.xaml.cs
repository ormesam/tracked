using Tracked.Contexts;
using Tracked.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Achievements {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementScreen : ContentPage {
        public AchievementScreen(MainContext context) {//, IAchievement achievement) {
            InitializeComponent();
        }

        public AchievementScreenViewModel ViewModel => BindingContext as AchievementScreenViewModel;

        private async void Ride_Tapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToRide(e.Item as Ride);
        }
    }
}