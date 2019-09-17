using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectRideScreen : ContentPage {
        public SelectRideScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new SelectRideScreenViewModel(context);
        }

        public SelectRideScreenViewModel ViewModel => BindingContext as SelectRideScreenViewModel;

        private async void Ride_Tapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.CreateSegment(Navigation, e.Item as Ride);
        }
    }
}