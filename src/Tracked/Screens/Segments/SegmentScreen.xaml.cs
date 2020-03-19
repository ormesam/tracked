using System;
using Shared.Dtos;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.Segments {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SegmentScreen : ContentPage {
        public SegmentScreen(SegmentScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public SegmentScreenViewModel ViewModel => BindingContext as SegmentScreenViewModel;

        private void Name_Tapped(object sender, EventArgs e) {
            ViewModel.ChangeName();
        }

        private async void Attempt_Tapped(object sender, ItemTappedEventArgs e) {
            await ViewModel.GoToAttempt(e.Item as SegmentAttemptOverviewDto);
        }

        private async void Delete_Clicked(object sender, EventArgs e) {
            bool delete = await DisplayAlert(
                "Delete Segment",
                "Are you sure you want to delete this segment?",
                "Yes",
                "No");

            if (delete) {
                await ViewModel.Delete();
                await Navigation.PopToRootAsync();
            }
        }
    }
}