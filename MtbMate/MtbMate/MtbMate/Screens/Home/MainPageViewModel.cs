﻿using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Screens;
using MtbMate.Screens.Bluetooth;
using MtbMate.Screens.Ride;
using MtbMate.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MtbMate.Home
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(MainContext context) : base(context)
        {
        }

        public ObservableCollection<RideModel> Rides => Context.Model.Rides
            .OrderByDescending(i => i.Start)
            .ToObservable();

        public bool HasRides => Rides.Any();

        public async Task GoToBluetoothSettings(INavigation nav)
        {
            await nav.PushAsync(new BluetoothSetupScreen(Context));
        }

        public async Task GoToCreateRide(INavigation nav)
        {
            await nav.PushAsync(new RideScreen(Context));
        }

        public async Task GoToReview(INavigation nav, RideModel ride)
        {
            await nav.PushAsync(new ReviewScreen(Context, ride));
        }
    }
}