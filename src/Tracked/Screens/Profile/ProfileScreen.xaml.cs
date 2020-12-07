using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Profile {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileScreen : ContentPage {
        public ProfileScreen(ProfileScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}