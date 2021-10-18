using System;
using System.Threading.Tasks;
using Tracked.Contexts;
using Tracked.Models;

namespace Tracked.Screens.Settings {
    public class SettingsScreenViewModel : TabbedViewModelBase {
        private event EventHandler<EventArgs> JumpsEnabled;

        public SettingsScreenViewModel(MainContext context) : base(context, TabItemType.Settings) {
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

        public async Task GoToBlockedUsers() {
            await Context.UI.GoToBlockedUsersAsync();
        }
    }
}
