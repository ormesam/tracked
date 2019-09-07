using MtbMate.Contexts;
using MtbMate.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Master {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage {
        public MainMenu(MainContext context) {
            InitializeComponent();
            BindingContext = new MainMenuViewModel(context);
        }

        private async void Menu_ItemTapped(object sender, ItemTappedEventArgs e) {
            ExtendedMenuItem menuItem = e.Item as ExtendedMenuItem;

            await menuItem?.OnClick();

            navMenu.SelectedItem = null;
        }
    }
}