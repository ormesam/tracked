using System;
using MtbMate.Contexts;
using MtbMate.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Bluetooth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothSetupScreen : ContentPage
    {
        public BluetoothSetupScreen(MainContext context)
        {
            InitializeComponent();
            BindingContext = new BluetoothSetupScreenViewModel(context);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDeviceList();
        }

        public BluetoothSetupScreenViewModel ViewModel => BindingContext as BluetoothSetupScreenViewModel;

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ViewModel.ConnectToDevice(e.Item as DeviceInfo);
        }

        private void Disconnect_Clicked(object sender, EventArgs e)
        {
            ViewModel.DisconnectDevice();
        }

        private void TurnBluetoothOn_Clicked(object sender, EventArgs e)
        {
            ViewModel.TurnBluetoothOn();
        }
    }
}