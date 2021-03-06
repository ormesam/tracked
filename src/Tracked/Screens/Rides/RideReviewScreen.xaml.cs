﻿
using System;
using Shared.Dtos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Rides {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RideReviewScreen : ContentPage {
        public RideReviewScreen(RideReviewScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing() {
            if (ViewModel.CanCreateTrail && page.ToolbarItems.Count == 1) {
                var item = new ToolbarItem {
                    Text = "Create Trail",
                    Order = ToolbarItemOrder.Secondary,
                };

                item.Clicked += CreateTrail_Clicked; ;

                page.ToolbarItems.Add(item);
            }

            base.OnAppearing();

            mapControl.CreateMap();
        }

        public RideReviewScreenViewModel ViewModel => BindingContext as RideReviewScreenViewModel;

        private async void SpeedAnalysis_Clicked(object sender, EventArgs e) {
            await ViewModel.GoToSpeedAnalysis();
        }

        private async void Delete_Clicked(object sender, EventArgs e) {
            bool delete = await DisplayAlert(
                "Delete Ride",
                "Are you sure you want to delete this ride? You will lose all trail attempts and achievements.",
                "Yes",
                "No");

            if (delete) {
                await ViewModel.Delete();
                await Navigation.PopToRootAsync();
            }
        }

        private async void CreateTrail_Clicked(object sender, EventArgs e) {
            await ViewModel.CreateTrail();
        }

        private async void Attempt_ItemTapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToTrailScreen(e.Item as TrailAttemptDto);
        }

        private async void Map_Tapped(object sender, EventArgs e) {
            await ViewModel.GoToMapScreen();
        }
    }
}