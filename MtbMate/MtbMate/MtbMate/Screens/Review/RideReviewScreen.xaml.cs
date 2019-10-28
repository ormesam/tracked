
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Screens.Review.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RideReviewScreen : TabbedPage {
        public RideReviewScreen(MainContext context, IRide ride) {
            InitializeComponent();
            BindingContext = new RideReviewScreenViewModel(context, ride);

            tabPage.Children.Add(new ReviewControl());

            if (ride.ShowAttempts) {
                tabPage.Children.Add(new SegmentsControl());
            }

            tabPage.Children.Add(new AnalysisControl());
        }
    }
}