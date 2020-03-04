﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
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
            ViewModel.Init(map);
        }

        private async void Map_MapClicked(object sender, MapClickedEventArgs e) {
            await ViewModel.OnMapClicked(e);
        }

        private void ChangeLayer_Pressed(object sender, EventArgs e) {
            picker.Focus();
        }
    }
}