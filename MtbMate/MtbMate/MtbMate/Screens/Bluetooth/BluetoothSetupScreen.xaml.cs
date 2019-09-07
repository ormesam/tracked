using System;
using System.Threading.Tasks;
using MtbMate.Contexts;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Bluetooth {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothSetupScreen : ContentPage
    {
        public BluetoothSetupScreen(MainContext context)
        {
            InitializeComponent();
            BindingContext = new BluetoothSetupScreenViewModel(context);
        }

        public BluetoothSetupScreenViewModel ViewModel => BindingContext as BluetoothSetupScreenViewModel;

        private void StartScan_Clicked(object sender, EventArgs e)
        {
            Task.Run(ViewModel.TryStartScanning);
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is IDevice device)
            {
                await ViewModel.ConnectToDevice(device);
            }
        }

        private async void Disconnect_Clicked(object sender, EventArgs e)
        {
            await ViewModel.DisconnectDevice();
        }
    }
}