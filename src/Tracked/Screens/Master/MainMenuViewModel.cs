using System.Collections.ObjectModel;
using Tracked.Contexts;
using Tracked.Controls;

namespace Tracked.Screens.Master {
    public class MainMenuViewModel : ViewModelBase {
        public MainMenuViewModel(MainContext context) : base(context) {
            MenuItems = new ObservableCollection<ExtendedMenuItem>
            {
                 new ExtendedMenuItem
                 {
                      Title = "Rides",
                      OnClick = Context.UI.GoToMainPageAsync,
                 },
                 new ExtendedMenuItem
                 {
                      Title = "Trails",
                      OnClick = Context.UI.GoToExploreTrailsScreenAsync,
                 },
                 new ExtendedMenuItem
                 {
                      Title = "Bluetooth",
                      OnClick = Context.UI.GoToBluetoothScreenAsync,
                 },
            };
        }

        public ObservableCollection<ExtendedMenuItem> MenuItems { get; }
    }
}