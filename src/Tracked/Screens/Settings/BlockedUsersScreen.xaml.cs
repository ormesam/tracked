using Shared.Dtos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Settings {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BlockedUsersScreen : ContentPage {
        public BlockedUsersScreen(BlockedUsersScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public BlockedUsersScreenViewModel ViewModel => BindingContext as BlockedUsersScreenViewModel;

        private async void Item_Tapped(object sender, ItemTappedEventArgs e) {
            var user = e.Item as BlockedUserDto;

            if (user != null) {
                bool block = await DisplayAlert(
                    "Block User",
                    $"Are you sure you want to unblock {user.UserName}?",
                    "Yes",
                    "No");

                if (block) {
                    await ViewModel.Unblock(user);
                }
            }
        }
    }
}