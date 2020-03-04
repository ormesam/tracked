using Tracked.Contexts;
using Tracked.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccelerometerReadingsScreen : ContentPage {
        public AccelerometerReadingsScreen(MainContext context, IRide ride) {
            InitializeComponent();
            BindingContext = new AccelerometerReadingsScreenViewModel(context, ride);
        }
    }
}