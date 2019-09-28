using System;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewScreen : ContentPage {
        public ReviewScreen(MainContext context, Ride ride) {
            InitializeComponent();
            BindingContext = new ReviewScreenViewModel(context, ride);
        }

        public ReviewScreenViewModel ViewModel => BindingContext as ReviewScreenViewModel;

        private async void Export_Clicked(object sender, EventArgs e) {
            await ViewModel.Export();
        }

        private async void Delete_Clicked(object sender, EventArgs e) {
            await ViewModel.Delete();

            await Navigation.PopToRootAsync();
        }

        private void Name_Tapped(object sender, EventArgs e) {
            ViewModel.ChangeName();
        }

        private async void ExportLocation_Clicked(object sender, EventArgs e) {
            await ViewModel.ExportLocation();
        }

        private async void Attempt_Tapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToAttempt(Navigation, e.Item as SegmentAttempt);
        }

        private async void Attempt_Tapped(object sender, EventArgs e) {
            var item = (sender as View).BindingContext;

            await ViewModel.GoToAttempt(Navigation, item as SegmentAttempt);
        }
    }
}