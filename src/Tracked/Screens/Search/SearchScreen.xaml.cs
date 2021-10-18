using System;
using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Search {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchScreen : ContentPage {
        public SearchScreen(MainContext context) {
            InitializeComponent();
            BindingContext = new SearchScreenViewModel(context);
        }

        public SearchScreenViewModel ViewModel => BindingContext as SearchScreenViewModel;

        private async void Result_ItemTapped(object sender, ItemTappedEventArgs e) {
            var user = e.Item as UserSearchDto;

            if (user != null) {
                await ViewModel.GoToUser(user.UserId);
            }
        }

        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e) {
            await ViewModel.SearchAsync();
        }
    }
}