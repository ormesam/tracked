using System.ComponentModel;
using Xamarin.Forms;

namespace MtbMate
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        public MainPageViewModel ViewModel => BindingContext as MainPageViewModel;

        private async void Start_Clicked(object sender, System.EventArgs e)
        {
            await ViewModel.Start();
        }

        private async void Stop_Clicked(object sender, System.EventArgs e)
        {
            await ViewModel.Stop();
        }
    }
}
