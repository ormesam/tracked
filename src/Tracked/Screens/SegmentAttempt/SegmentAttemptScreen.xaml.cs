
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Screens.SegmentAttempt {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SegmentAttemptScreen : ContentPage {
        public SegmentAttemptScreen(SegmentAttemptScreenViewModel viewModel) {
            InitializeComponent();
            BindingContext = viewModel;
        }

        public SegmentAttemptScreenViewModel ViewModel => BindingContext as SegmentAttemptScreenViewModel;

        private async void SpeedAnalysis_Clicked(object sender, EventArgs e) {
            await ViewModel.GoToSpeedAnalysis();
        }
    }
}