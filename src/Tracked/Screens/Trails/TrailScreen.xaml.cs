using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Trails {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrailScreen : ContentPage {
        public TrailScreen(TrailScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public TrailScreenViewModel ViewModel => BindingContext as TrailScreenViewModel;

        protected override void OnAppearing() {
            base.OnAppearing();

            mapControl.CreateMap();
        }

        private async void ChangeName_Clicked(object sender, EventArgs e) {
            await ViewModel.ChangeName();
        }

        private async void Delete_Clicked(object sender, EventArgs e) {
            bool delete = await DisplayAlert(
                "Delete Trail",
                "Are you sure you want to delete this trail?",
                "Yes",
                "No");

            if (delete) {
                await ViewModel.Delete();
                await Navigation.PopToRootAsync();
            }
        }
    }
}