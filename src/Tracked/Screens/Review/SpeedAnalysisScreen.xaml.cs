using System.Collections.Generic;
using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpeedAnalysisScreen : ContentPage {
        public SpeedAnalysisScreen(MainContext context, IList<RideLocationDto> rideLocations) {
            InitializeComponent();
            BindingContext = new SpeedAnalysisScreenViewModel(context, rideLocations);
        }

        public SpeedAnalysisScreenViewModel ViewModel => BindingContext as SpeedAnalysisScreenViewModel;
    }
}