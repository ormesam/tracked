using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSegmentScreen : ContentPage
    {
        public CreateSegmentScreen(MainContext context)
        {
            InitializeComponent();
            BindingContext = new CreateSegmentScreenViewModel(context);
        }

        public CreateSegmentScreenViewModel ViewModel => BindingContext as CreateSegmentScreenViewModel;

        private void Ride_Tapped(object sender, ItemTappedEventArgs e)
        {
            ViewModel.SelectedRide = e.Item as RideModel;
        }
    }
}