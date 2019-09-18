﻿using System;
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

        protected override void OnAppearing() {
            base.OnAppearing();

            Task.Run(async () => {
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

                var pin = new Position(position.Latitude, position.Longitude);

                Device.BeginInvokeOnMainThread(() => {
                    Map.MoveToRegion(MapSpan.FromCenterAndRadius(pin, Distance.FromMiles(0.5)));
                });
            });
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