using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Settings.Modals {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JumpAboutModal : ContentPage {
        public JumpAboutModal(MainContext context) {
            InitializeComponent();
            BindingContext = new JumpAboutModelViewModel(context);
        }

        public JumpAboutModelViewModel ViewModel => BindingContext as JumpAboutModelViewModel;

        private async void Ok_Tapped(object sender, System.EventArgs e) {
            await Navigation.PopModalAsync();
        }

        private async void More_Tapped(object sender, System.EventArgs e) {
            await ViewModel.GoToAboutJump();
        }
    }
}