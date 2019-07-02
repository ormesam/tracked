using System;
using System.ComponentModel;
using MtbMate.Contexts;
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

        private async void Bluetooth_Clicked(object sender, EventArgs e)
        {
            await ViewModel.GoToBluetoothSettings(Navigation);
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await ViewModel.CreateRide();
        }
    }
}
