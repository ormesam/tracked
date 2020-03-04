using System.Collections.Generic;
using Tracked.Contexts;
using Tracked.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapScreen : ContentPage {
        public MapScreen(MainContext context, string title, IList<MapLocation> locations, bool showRideFeatures) {
            InitializeComponent();
            BindingContext = new MapScreenViewModel(context, title, locations, showRideFeatures);
        }

        public MapScreenViewModel ViewModel => BindingContext as MapScreenViewModel;
    }
}