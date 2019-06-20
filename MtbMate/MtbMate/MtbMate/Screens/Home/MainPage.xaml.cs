﻿using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace MtbMate.Home
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        public MainPageViewModel ViewModel => BindingContext as MainPageViewModel;

        private async void Start_Clicked(object sender, EventArgs e)
        {
            await ViewModel.Start();
        }

        private async void Stop_Clicked(object sender, EventArgs e)
        {
            await ViewModel.Stop();
        }

        private async void Export_Clicked(object sender, EventArgs e)
        {
            await ViewModel.Export();
        }

        private async void Bluetooth_Clicked(object sender, EventArgs e)
        {
            await ViewModel.GoToBluetoothSettings(Navigation);
        }
    }
}