using System;
using System.Threading.Tasks;
using MtbMate.Contexts;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RideScreen : ContentPage {
        public RideScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new RideScreenViewModel(context);
        }

        public RideScreenViewModel ViewModel => BindingContext as RideScreenViewModel;

        private async void Start_Clicked(object sender, EventArgs e) {
            await ViewModel.Start();
        }

        private async void Stop_Clicked(object sender, EventArgs e) {
            await ViewModel.Stop(Navigation);
        }
    }
}