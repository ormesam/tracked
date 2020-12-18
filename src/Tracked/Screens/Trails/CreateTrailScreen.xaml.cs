using System;
using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Trails {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateTrailScreen : ContentPage {
        public CreateTrailScreen(MainContext context, RideDto ride) {
            InitializeComponent();
            BindingContext = new CreateTrailScreenViewModel(context, ride);
        }

        public CreateTrailScreenViewModel ViewModel => BindingContext as CreateTrailScreenViewModel;

        protected override void OnAppearing() {
            base.OnAppearing();

            mapControl.CreateMap();
        }

        private async void Save_Clicked(object sender, EventArgs e) {
            await ViewModel.Save(Navigation);
        }
    }
}