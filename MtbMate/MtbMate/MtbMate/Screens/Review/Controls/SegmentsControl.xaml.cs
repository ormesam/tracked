using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review.Controls {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SegmentsControl : ContentPage {
        public SegmentsControl() {
            InitializeComponent();
        }

        public RideReviewScreenViewModel ViewModel => BindingContext as RideReviewScreenViewModel;

        private async void Attempt_Tapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToAttempt(Navigation, e.Item as SegmentAttempt);
        }
    }
}