using System;
using Tracked.Contexts;
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

            await ViewModel.StartLocationListening();

            ViewModel.OnPropertyChanged();
        }

        protected override async void OnDisappearing() {
            await ViewModel.StopLocationListening();

            base.OnDisappearing();
        }

        private void Start_Clicked(object sender, EventArgs e) {
            ViewModel.Start();
        }

        private async void Stop_Clicked(object sender, EventArgs e) {
            await ViewModel.Stop();
            await Navigation.PopToRootAsync();
        }

        private void JumpToggle_Clicked(object sender, EventArgs e) {
            ViewModel.ToggleJumpDetection();
        }
    }
}