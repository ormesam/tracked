using System;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Segments {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSegmentScreen : ContentPage {
        public CreateSegmentScreen(MainContext context, RideDto ride) {
            InitializeComponent();
            BindingContext = new CreateSegmentScreenViewModel(context, ride);
        }

        public CreateSegmentScreenViewModel ViewModel => BindingContext as CreateSegmentScreenViewModel;

        private void Save_Clicked(object sender, EventArgs e) {
            ViewModel.Save(Navigation);
        }
    }
}