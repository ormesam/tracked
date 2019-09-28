using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MtbMate.Controls {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapControl : ContentView {
        public MapControl() {
            InitializeComponent();

            LayoutChanged += MapControl_LayoutChanged;
        }

        private void MapControl_LayoutChanged(object sender, EventArgs e) {
            Device.BeginInvokeOnMainThread(() => {
                CustomMap map = new CustomMap(MapSpan.FromCenterAndRadius(ViewModel.InitialLocation, Distance.FromMiles(0.25)));
                map.SetBinding(Map.IsShowingUserProperty, nameof(MapControlViewModel.IsShowingUser));
                map.SetBinding(CustomMap.RouteCoordinatesProperty, nameof(MapControlViewModel.Locations));
                map.SetBinding(CustomMap.ShowSpeedProperty, nameof(MapControlViewModel.ShowSpeed));
                map.SetBinding(Map.HasScrollEnabledProperty, nameof(MapControlViewModel.HasScrollEnabled));

                map.MapClicked += async (s, ev) => {
                    await ViewModel.GoToMapScreenAsync(Navigation);
                };

                grid.Children.Insert(0, map);
            });
        }

        public MapControlViewModel ViewModel => BindingContext as MapControlViewModel;
    }
}