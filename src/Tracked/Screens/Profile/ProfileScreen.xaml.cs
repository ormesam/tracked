﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Profile {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileScreen : ContentPage {
        public ProfileScreen(ProfileScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public ProfileScreenViewModel ViewModel => BindingContext as ProfileScreenViewModel;

        private async void Block_Clicked(object sender, EventArgs e) {
            bool block = await DisplayAlert(
                "Block User",
                "Are you sure you want to block this user?",
                "Yes",
                "No");

            if (block) {
                await ViewModel.Block();
            }
        }
    }
}