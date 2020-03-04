using Tracked.Contexts;
using Tracked.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Master {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage {
        public MainMenu(MainContext context) {
            InitializeComponent();
            BindingContext = new MainMenuViewModel(context);
        }

        public MainMenuViewModel ViewModel => BindingContext as MainMenuViewModel;

        private async void Menu_ItemTapped(object sender, ItemTappedEventArgs e) {
            ExtendedMenuItem menuItem = e.Item as ExtendedMenuItem;

            await menuItem?.OnClick();

            navMenu.SelectedItem = null;
        }

        private void ConnectToGoogle_Clicked(object sender, System.EventArgs e) {
            ViewModel.ConnectToGoogle();
        }
    }
}