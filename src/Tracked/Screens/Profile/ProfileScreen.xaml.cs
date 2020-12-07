using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Profile {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileScreen : ContentPage {
        public ProfileScreen(ProfileScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public ProfileScreenViewModel ViewModel => BindingContext as ProfileScreenViewModel;

        protected override void OnAppearing() {
            if (ViewModel.IsCurrentUser) {
                var item = new ToolbarItem {
                    Text = "Settings",
                    Order = ToolbarItemOrder.Primary,
                };

                item.Clicked += Settings_Tapped; ;

                page.ToolbarItems.Add(item);
            }

            base.OnAppearing();
        }

        private async void Settings_Tapped(object sender, EventArgs e) {
            await ViewModel.GoToSettings();
        }
    }
}