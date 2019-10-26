
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RideReviewScreen : TabbedPage {
        public RideReviewScreen(MainContext context, Ride ride) {
            InitializeComponent();
            BindingContext = new RideReviewScreenViewModel(context, ride);
        }
    }
}