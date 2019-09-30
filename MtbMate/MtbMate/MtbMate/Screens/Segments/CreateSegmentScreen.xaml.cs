using System;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSegmentScreen : ContentPage {
        public CreateSegmentScreen(MainContext context, Ride ride) {
            InitializeComponent();
            BindingContext = new CreateSegmentScreenViewModel(context, ride);
        }

        public CreateSegmentScreenViewModel ViewModel => BindingContext as CreateSegmentScreenViewModel;

        private void Save_Clicked(object sender, EventArgs e) {
            ViewModel.Save(Navigation);
        }
    }
}