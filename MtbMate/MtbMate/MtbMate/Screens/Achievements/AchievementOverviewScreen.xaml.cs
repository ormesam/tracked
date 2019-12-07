using System;
using MtbMate.Achievements;
using MtbMate.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementOverviewScreen : ContentPage {
        public AchievementOverviewScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new AchievementOverviewScreenViewModel(context);
        }

        public AchievementOverviewScreenViewModel ViewModel => BindingContext as AchievementOverviewScreenViewModel;

        private async void Achievements_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToAchievement(Navigation, e.Item as IAchievement);
        }
    }
}