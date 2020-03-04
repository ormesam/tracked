using System;
using System.Collections.ObjectModel;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Models;

namespace Tracked.Screens.Master {
    public class MainMenuViewModel : ViewModelBase {
        private bool isLoggingIn;

        public MainMenuViewModel(MainContext context) : base(context) {
            Context.Security.LoggedInStatusChanged += Security_UserChanged;

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
                      OnClick = Model.Instance.RunUtilityAsync,
                 },
#endif
            };
        }

        public ObservableCollection<ExtendedMenuItem> MenuItems { get; }

        public string LoggedInText => Context.Security.Name ?? "Connected to Google";

        private void Security_UserChanged(object sender, EventArgs e) {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(LoggedInText));

            isLoggingIn = false;
        }

        public void ConnectToGoogle() {
            if (isLoggingIn) {
                return;
            }

            isLoggingIn = true;

            Context.Security.ConnectToGoogle();
        }
    }
}