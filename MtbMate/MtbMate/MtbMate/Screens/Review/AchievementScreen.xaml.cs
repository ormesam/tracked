using System;
using MtbMate.Achievements;
using MtbMate.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementScreen : ContentPage {
        public AchievementScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new AchievementScreenViewModel(context);
        }

        public AchievementScreenViewModel ViewModel => BindingContext as AchievementScreenViewModel;

        private async void Achievements_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToRide(Navigation, e.Item as IAchievement);
        }

        private async void RecompareRides_Clicked(object sender, EventArgs e) {
            await ViewModel.ReCompareRides();
        }
    }
}