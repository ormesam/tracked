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
                      Title = "Segments",
                      OnClick = Context.UI.GoToExploreSegmentsScreenAsync,
                 },
                 new ExtendedMenuItem
                 {
                      Title = "Achievements",
                      OnClick = Context.UI.GoToAchievementOverviewScreenAsync,
                 },
                 new ExtendedMenuItem
                 {
                      Title = "Bluetooth",
                      OnClick = Context.UI.GoToBluetoothScreenAsync,
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
                      OnClick = Context.Model.RunUtilityAsync,
                 },
#endif
            };
        }

        public ObservableCollection<ExtendedMenuItem> MenuItems { get; }
    }
}