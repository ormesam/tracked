using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Settings {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JumpAboutScreen : ContentPage {
        public JumpAboutScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new JumpAboutScreenViewModel(context);
        }
    }
}