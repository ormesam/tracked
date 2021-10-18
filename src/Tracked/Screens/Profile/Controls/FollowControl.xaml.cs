using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Profile.Controls {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FollowControl : ContentView {
        public FollowControl() {
            InitializeComponent();
        }

        public ProfileScreenViewModel ViewModel => BindingContext as ProfileScreenViewModel;

        private async void Follow_Clicked(object sender, EventArgs e) {
            await ViewModel.Follow();
        }

        private async void Unfollow_Clicked(object sender, EventArgs e) {
            await ViewModel.Unfollow();
        }
    }
}