﻿using Tracked.Contexts;
using Tracked.Screens.Login;

namespace Tracked.Screens.Settings {
    public class SettingsScreenViewModel : ViewModelBase {
        public SettingsScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => "Settings";

        public void DisconnectFromGoogle() {
            Context.Security.Logout();

            App.Current.MainPage = new LoginScreen(Context, false);
        }
    }
}
