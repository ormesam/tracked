﻿
using System;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RideReviewScreen : ContentPage {
        public RideReviewScreen(MainContext context, IRide ride) {
            InitializeComponent();
            BindingContext = new RideReviewScreenViewModel(context, ride);
        }

        public RideReviewScreenViewModel ViewModel => BindingContext as RideReviewScreenViewModel;

        private async void Delete_Clicked(object sender, EventArgs e) {
            await ViewModel.Delete();

            await Navigation.PopToRootAsync();
        }

        private async void JumpBreakdown_Clicked(object sender, EventArgs e) {
            await ViewModel.ViewJumpBreakdown(Navigation);
        }

        private async void Sync_Clicked(object sender, EventArgs e) {
            await ViewModel.Sync();
        }

        private async void RecalculateJumps_Clicked(object sender, EventArgs e) {
            await ViewModel.RecalculateJumps();
        }

        private async void SpeedAnalysis_Clicked(object sender, EventArgs e) {
            await ViewModel.GoToSpeedAnalysis(Navigation);
        }

        private async void Attempt_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToAttempt(Navigation, e.Item as SegmentAttempt);
        }
    }
}