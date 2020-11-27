using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Profile {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileScreen : ContentPage {
        public ProfileScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new ProfileScreenViewModel(context);
        }
    }
}