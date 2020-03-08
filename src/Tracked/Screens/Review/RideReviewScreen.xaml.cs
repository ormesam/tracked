
using System;
using Tracked.Contexts;
using Tracked.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RideReviewScreen : ContentPage {
        public RideReviewScreen(MainContext context, RideReviewScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public RideReviewScreenViewModel ViewModel => BindingContext as RideReviewScreenViewModel;

        private async void JumpBreakdown_Clicked(object sender, EventArgs e) {
            await ViewModel.ViewJumpBreakdown();
        }

        private async void SpeedAnalysis_Clicked(object sender, EventArgs e) {
            await ViewModel.GoToSpeedAnalysis();
        }

        private async void Attempt_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToAttempt(e.Item as SegmentAttempt);
        }
    }
}