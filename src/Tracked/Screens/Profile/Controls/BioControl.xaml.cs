using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Profile.Controls {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BioControl : ContentView {
        public BioControl() {
            InitializeComponent();
        }

        public ProfileScreenViewModel ViewModel => BindingContext as ProfileScreenViewModel;

        private async void Bio_Tapped(object sender, System.EventArgs e) {
            await ViewModel.EditBio();
        }
    }
}