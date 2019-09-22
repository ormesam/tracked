using System.Collections.ObjectModel;
using MtbMate.Contexts;
using MtbMate.Controls;
using MtbMate.Models;

namespace MtbMate.Screens.Master {
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
#if DEBUG
                new ExtendedMenuItem
                 {
                      Title = "Run Utility",
                      OnClick = Model.Instance.RunUtilityAsync,
                 },
#endif
            };
        }

        public ObservableCollection<ExtendedMenuItem> MenuItems { get; }
    }
}