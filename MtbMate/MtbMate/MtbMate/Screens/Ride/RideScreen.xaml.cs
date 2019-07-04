using MtbMate.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Ride
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RideScreen : ContentPage
    {
        public RideScreen(MainContext context)
        {
            InitializeComponent();
            BindingContext = new RideScreenViewModel(context);
        }

        public RideScreenViewModel ViewModel => BindingContext as RideScreenViewModel;

        private async void Start_Clicked(object sender, System.EventArgs e)
        {
            await ViewModel.Start();
        }

        private async void Stop_Clicked(object sender, System.EventArgs e)
        {
            await ViewModel.Stop();
        }

        private async void Export_Clicked(object sender, System.EventArgs e)
        {
            await ViewModel.Export();
        }

        private async void Save_Clicked(object sender, System.EventArgs e)
        {
            await ViewModel.Save();

            await Navigation.PopToRootAsync();
        }
    }
}