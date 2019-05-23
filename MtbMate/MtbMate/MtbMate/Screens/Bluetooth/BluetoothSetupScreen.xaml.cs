using System;
using System.Threading.Tasks;
using MtbMate.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Bluetooth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothSetupScreen : ContentPage
    {
        public BluetoothSetupScreen()
        {
            InitializeComponent();
            BindingContext = new BluetoothSetupScreenViewModel();
        }

        public BluetoothSetupScreenViewModel ViewModel => BindingContext as BluetoothSetupScreenViewModel;

        private void StartScan_Clicked(object sender, EventArgs e)
        {
            Task.Run(ViewModel.TryStartScanning);
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is DeviceInfo deviceInfo)
            {
                await ViewModel.ConnectToDevice(deviceInfo);
            }
        }
    }
}