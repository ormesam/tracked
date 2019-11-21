using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpeedAnalysisScreen : ContentPage {
        public SpeedAnalysisScreen(MainContext context, IRide ride) {
            InitializeComponent();
            BindingContext = new SpeedAnalysisScreenViewModel(context, ride);
        }

        public SpeedAnalysisScreenViewModel ViewModel => BindingContext as SpeedAnalysisScreenViewModel;
    }
}