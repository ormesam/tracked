using System;
using MtbMate.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordScreen : ContentPage {
        public RecordScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new RecordScreenViewModel(context);
        }

        public RecordScreenViewModel ViewModel => BindingContext as RecordScreenViewModel;

        private async void Start_Clicked(object sender, EventArgs e) {
            await ViewModel.Start();
        }

        private async void Stop_Clicked(object sender, EventArgs e) {
            await ViewModel.Stop(Navigation);
        }
    }
}