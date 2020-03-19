using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Login {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginScreen : ContentPage {
        public LoginScreen(Contexts.MainContext context) {
            InitializeComponent();
            BindingContext = new LoginScreenViewModel(context);
        }

        public LoginScreenViewModel ViewModel => BindingContext as LoginScreenViewModel;

        private void LoginWithGoogle_Clicked(object sender, EventArgs e) {
            ViewModel.LoginWithGoogle();
        }
    }
}