using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Controls {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapControl : ContentView {
        public MapControl() {
            InitializeComponent();

            LayoutChanged += MapControl_LayoutChanged;
        }

        public MapControlViewModel ViewModel => BindingContext as MapControlViewModel;

        private void MapControl_LayoutChanged(object sender, EventArgs e) {
            Device.BeginInvokeOnMainThread(() => {
                if (MapContainer.Children.Any()) {
                    return;
                }

                MapContainer.Children.Add(ViewModel.CreateMap());
            });
        }

        private void ChangeLayer_Pressed(object sender, EventArgs e) {
            picker.Focus();
        }

        private async void Map_Tapped(object sender, EventArgs e) {
            await ViewModel.OnMappedTapped(sender, e);
        }
    }
}