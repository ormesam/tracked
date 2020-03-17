
using System;
using Shared.Dtos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RideReviewScreen : ContentPage {
        public RideReviewScreen(RideReviewScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public RideReviewScreenViewModel ViewModel => BindingContext as RideReviewScreenViewModel;

        private async void SpeedAnalysis_Clicked(object sender, EventArgs e) {
            await ViewModel.GoToSpeedAnalysis();
        }

        private async void Attempt_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToAttempt(e.Item as SegmentAttemptOverviewDto);
        }

        private async void Delete_Clicked(object sender, EventArgs e) {
            bool delete = await DisplayAlert(
                "Delete Ride",
                "Are you sure you want to delete this ride? You will lose all segment attempts and achievements.",
                "Yes",
                "No");

            if (delete) {
                await ViewModel.Delete();
                await Navigation.PopToRootAsync();
            }
        }

        private async void CreateSegment_Clicked(object sender, EventArgs e) {
            await ViewModel.CreateSegment();
        }
    }
}