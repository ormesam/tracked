using System;
using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Login {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginScreen : ContentPage {
        private bool autoLogin;

        public LoginScreen(MainContext context, bool autoLogin) {
            InitializeComponent();
            BindingContext = new LoginScreenViewModel(context);

            this.autoLogin = autoLogin;
        }

        protected override async void OnAppearing() {
            base.OnAppearing();

            if (autoLogin) {
                await ViewModel.TryLogin();
            }
        }

        public LoginScreenViewModel ViewModel => BindingContext as LoginScreenViewModel;

        private async void LoginWithGoogle_Clicked(object sender, EventArgs e) {
            await ViewModel.TryLogin();
        }
    }
}