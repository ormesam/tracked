using MtbMate.Contexts;
using MtbMate.Controls;
using System.Collections.ObjectModel;

namespace MtbMate.Screens.Master
{
    public class MainMenuViewModel : ViewModelBase
    {
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
                      Title = "Bluetooth",
                      OnClick = Context.UI.GoToBluetoothScreenAsync,
                 },
                 new ExtendedMenuItem
                 {
                      Title = "Segments",
                      OnClick = Context.UI.GoToExploreSegmentsScreenAsync,
                 },
                 new ExtendedMenuItem
                 {
                      Title = "Settings",
                      OnClick = Context.UI.GoToSettingsScreenAsync,
                 },
            };
        }

        public ObservableCollection<ExtendedMenuItem> MenuItems { get; }
    }
}