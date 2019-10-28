using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Screens.Review.Controls {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewControl : ContentPage {
        public ReviewControl() {
            InitializeComponent();
        }

        public RideReviewScreenViewModel ViewModel => BindingContext as RideReviewScreenViewModel;

        private async void Delete_Clicked(object sender, EventArgs e) {
            await ViewModel.Delete();

            await Navigation.PopToRootAsync();
        }

        private void Name_Tapped(object sender, EventArgs e) {
            ViewModel.ChangeName();
        }

        private async void JumpBreakdown_Clicked(object sender, EventArgs e) {
            await ViewModel.ViewJumpBreakdown(Navigation);
        }
    }
}