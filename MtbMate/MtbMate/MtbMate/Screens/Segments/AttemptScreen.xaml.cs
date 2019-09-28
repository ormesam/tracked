using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Segments {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttemptScreen : ContentPage {
        public AttemptScreen(MainContext context, SegmentAttempt attempt) {
            InitializeComponent();
            BindingContext = new AttemptScreenViewModel(context, attempt);
        }

        public AttemptScreenViewModel ViewModel => BindingContext as AttemptScreenViewModel;
    }
}