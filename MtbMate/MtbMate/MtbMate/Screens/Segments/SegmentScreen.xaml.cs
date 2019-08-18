using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SegmentScreen : ContentPage
    {
        public SegmentScreen(MainContext context, SegmentModel segment)
        {
            InitializeComponent();
            BindingContext = new SegmentScreenViewModel(context, segment);
        }

        public SegmentScreenViewModel ViewModel => BindingContext as SegmentScreenViewModel;
    }
}