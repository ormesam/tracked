using System.Collections.Generic;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapScreen : ContentPage {
        public MapScreen(MainContext context, string title, IList<MapLocation> locations, bool showSpeed) {
            InitializeComponent();
            BindingContext = new MapScreenViewModel(context, title, locations, showSpeed);
        }

        public MapScreenViewModel ViewModel => BindingContext as MapScreenViewModel;
    }
}