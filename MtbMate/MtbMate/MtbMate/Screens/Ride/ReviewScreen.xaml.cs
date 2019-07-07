using System;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Ride
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewScreen : ContentPage
    {
        public ReviewScreen(MainContext context, RideModel ride)
        {
            InitializeComponent();
            BindingContext = new ReviewScreenViewModel(context, ride);
        }

        public ReviewScreenViewModel ViewModel => BindingContext as ReviewScreenViewModel;

        private async void Export_Clicked(object sender, EventArgs e)
        {
            await ViewModel.Export();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await ViewModel.Delete();

            await Navigation.PopToRootAsync();
        }

        private void Name_Tapped(object sender, EventArgs e)
        {
            ViewModel.ChangeName();
        }
    }
}