﻿using System;
using Tracked.Screens;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Controls {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapControl : ContentView {
        private bool isInitialized;

        public MapControl() {
            InitializeComponent();
        }

        public MapViewModelBase ViewModel => BindingContext as MapViewModelBase;

        public void CreateMap() {
            if (isInitialized) {
                return;
            }

            mapContainer.Children.Add(ViewModel.CreateMap());

            isInitialized = true;
        }

        private void ChangeLayer_Pressed(object sender, EventArgs e) {
            picker.Focus();
        }
    }
}