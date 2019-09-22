using System;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SegmentScreen : ContentPage {
        public SegmentScreen(MainContext context, Segment segment) {
            InitializeComponent();
            BindingContext = new SegmentScreenViewModel(context, segment);

            Map.RouteCoordinates = PolyUtils.GetMapLocations(ViewModel.Locations);
        }

        public SegmentScreenViewModel ViewModel => BindingContext as SegmentScreenViewModel;

        protected override void OnAppearing() {
            base.OnAppearing();

            Task.Run(() => {
                Map.GoToLocations(ViewModel.Locations);
            });
        }

        private void Name_Tapped(object sender, EventArgs e) {
            ViewModel.ChangeName();
        }

        private async void Map_MapClicked(object sender, MapClickedEventArgs e) {
            await ViewModel.GoToMapScreen(Navigation);
        }

        private async void Delete_Clicked(object sender, EventArgs e) {
            await ViewModel.DeleteSegment(Navigation);
        }

        private async void Attempt_Tapped(object sender, EventArgs e) {
            var item = (sender as View).BindingContext;

            await ViewModel.GoToAttempt(Navigation, item as SegmentAttempt);
        }

        private async void RecompareRides_Clicked(object sender, EventArgs e) {
            await ViewModel.RecompareRides();
        }
    }
}