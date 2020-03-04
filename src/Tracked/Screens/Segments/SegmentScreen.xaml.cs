using System;
using Tracked.Contexts;
using Tracked.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Segments {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SegmentScreen : ContentPage {
        public SegmentScreen(MainContext context, Segment segment) {
            InitializeComponent();
            BindingContext = new SegmentScreenViewModel(context, segment);
        }

        public SegmentScreenViewModel ViewModel => BindingContext as SegmentScreenViewModel;

        private void Name_Tapped(object sender, EventArgs e) {
            ViewModel.ChangeName();
        }

        private async void Delete_Clicked(object sender, EventArgs e) {
            await ViewModel.DeleteSegment(Navigation);
        }

        private async void Attempt_Tapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToAttempt(e.Item as SegmentAttempt);
        }
    }
}