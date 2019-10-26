using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review.Controls {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnalysisControl : ContentPage {
        public AnalysisControl() {
            InitializeComponent();
        }

        public RideReviewScreenViewModel ViewModel => BindingContext as RideReviewScreenViewModel;
    }
}