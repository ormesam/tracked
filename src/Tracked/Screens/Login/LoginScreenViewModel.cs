using System;
using Tracked.Contexts;
using Tracked.Screens.Master;

namespace Tracked.Screens.Login {
    public class LoginScreenViewModel : ViewModelBase {
        public LoginScreenViewModel(MainContext context) : base(context) {
            Context.Security.LoggedInStatusChanged += Security_LoggedInStatusChanged;
        }

        private void Security_LoggedInStatusChanged(object sender, EventArgs e) {
            if (Context.Security.IsLoggedIn) {
                Context.Security.LoggedInStatusChanged -= Security_LoggedInStatusChanged;
                App.Current.MainPage = new MasterScreen(Context);
            }
        }

        public void LoginWithGoogle() {
            Context.Security.ConnectToGoogle();
        }
    }
}