using System;
using Tracked.Contexts;

namespace Tracked.Screens.Settings {
    public class SettingsScreenViewModel : ViewModelBase {
        private event EventHandler<EventArgs> JumpsEnabled;

        public SettingsScreenViewModel(MainContext context) : base(context) {
            JumpsEnabled += SettingsScreenViewModel_JumpsEnabled;
        }

        public override string Title => "Settings";

        public bool ShouldDetectJumps {
            get { return Context.Settings.ShouldDetectJumps; }
            set {
                if (Context.Settings.ShouldDetectJumps != value) {
                    Context.Settings.ShouldDetectJumps = value;

                    if (value) {
                        JumpsEnabled?.Invoke(null, null);
                    }
                }
            }
        }

        public void DisconnectFromGoogle() {
            Context.Security.Logout();
        }

        private async void SettingsScreenViewModel_JumpsEnabled(object sender, EventArgs e) {
            await Context.UI.ShowJumpAboutModal();
        }
    }
}
