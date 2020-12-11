using System;
using Tracked.Models;
using Tracked.Screens;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Controls {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabControl : ContentView {
        public TabControl() {
            InitializeComponent();
        }

        public TabbedViewModelBase ViewModel => BindingContext as TabbedViewModelBase;

        private async void TabItem_Tapped(object sender, EventArgs e) {
            var item = (sender as View).BindingContext as TabItem;

            if (item == null || item.IsSelected) {
                return;
            }

            await item.OnClick();
        }
    }
}