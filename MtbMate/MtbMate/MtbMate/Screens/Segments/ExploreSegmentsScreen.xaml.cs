using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExploreSegmentsScreen : ContentPage
    {
        public ExploreSegmentsScreen(MainContext context)
        {
            InitializeComponent();
            BindingContext = new ExploreSegmentsScreenViewModel(context);
        }

        public ExploreSegmentsScreenViewModel ViewModel => BindingContext as ExploreSegmentsScreenViewModel;

        private async void Add_Clicked(object sender, System.EventArgs e)
        {
            await ViewModel.AddSegment(Navigation);
        }

        private async void Segments_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await ViewModel.GoToSegment(Navigation, e.Item as SegmentModel);
        }
    }
}