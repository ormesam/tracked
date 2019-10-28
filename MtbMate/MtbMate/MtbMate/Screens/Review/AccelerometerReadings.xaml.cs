using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccelerometerReadingsScreen : ContentPage {
        public AccelerometerReadingsScreen(MainContext context, IRide ride) {
            InitializeComponent();
            BindingContext = new AccelerometerReadingsScreenViewModel(context, ride);
        }
    }
}