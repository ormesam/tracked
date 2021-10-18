using System;
using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Settings {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsScreen : ContentPage {
        public SettingsScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new SettingsScreenViewModel(context);
        }

        public SettingsScreenViewModel ViewModel => BindingContext as SettingsScreenViewModel;

        private void DisconnectFromGoogle_Click(object sender, EventArgs e) {
            ViewModel.DisconnectFromGoogle();
        }

        private async void BlockedUsers_Clicked(object sender, EventArgs e) {
            await ViewModel.GoToBlockedUsers();
        }
    }
}