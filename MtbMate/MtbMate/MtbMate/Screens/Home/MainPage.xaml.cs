using System;
using System.ComponentModel;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;

namespace MtbMate.Home
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage(MainContext context)
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(context);
        }

        public MainPageViewModel ViewModel => BindingContext as MainPageViewModel;

        protected override void OnAppearing()
        {
            ViewModel.Refresh();

            base.OnAppearing();
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await ViewModel.GoToCreateRide(Navigation);
        }

        private async void Ride_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await ViewModel.GoToReview(Navigation, e.Item as RideModel);
        }
    }
}
