using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpeedAnalysisScreen : ContentPage {
        public SpeedAnalysisScreen(MainContext context, RideDto ride) {
            InitializeComponent();
            BindingContext = new SpeedAnalysisScreenViewModel(context, ride);
        }

        public SpeedAnalysisScreenViewModel ViewModel => BindingContext as SpeedAnalysisScreenViewModel;
    }
}