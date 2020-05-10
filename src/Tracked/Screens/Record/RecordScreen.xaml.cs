using System;
using Tracked.Accelerometer;
using Tracked.Contexts;
using Tracked.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Record {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordScreen : ContentPage {
        public RecordScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new RecordScreenViewModel(context);
        }

        public RecordScreenViewModel ViewModel => BindingContext as RecordScreenViewModel;

        protected override async void OnAppearing() {
            base.OnAppearing();

            ViewModel.Context.GeoUtility.Start();

            if (ViewModel.IsAccelerometerRequired) {
                await AccelerometerUtility.Instance.TryConnect(ViewModel.Context.Settings.BluetoothDeviceId);
            }
        }

        protected override void OnDisappearing() {
            if (ViewModel.Status != RecordStatus.Running) {
                ViewModel.Context.GeoUtility.Stop();
            }

            base.OnDisappearing();
        }

        private async void Start_Clicked(object sender, EventArgs e) {
            await ViewModel.Start();
        }

        private async void Stop_Clicked(object sender, EventArgs e) {
            await ViewModel.Stop();
            await Navigation.PopToRootAsync();
        }
    }
}