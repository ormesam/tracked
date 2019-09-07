using System;
using MtbMate.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Settings {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsScreen : ContentPage {
        public SettingsScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new SettingsScreenViewModel(context);
        }

        public SettingsScreenViewModel ViewModel => BindingContext as SettingsScreenViewModel;

        private async void Save_Clicked(object sender, EventArgs e) {
            await ViewModel.Save(Navigation);
        }
    }
}