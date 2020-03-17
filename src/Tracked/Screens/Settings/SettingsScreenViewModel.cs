using System.Threading.Tasks;
using Tracked.Contexts;
using Tracked.Screens.Login;
using Xamarin.Forms;

namespace Tracked.Screens.Settings {
    public class SettingsScreenViewModel : ViewModelBase {
        private bool detectJumps;

        public SettingsScreenViewModel(MainContext context) : base(context) {
            detectJumps = context.Settings.DetectJumps;
        }

        public override string Title => "Settings";

        public bool DetectJumps {
            get { return detectJumps; }
            set {
                if (detectJumps != value) {
                    detectJumps = value;
                    OnPropertyChanged(nameof(DetectJumps));
                }
            }
        }

        public async Task Save(INavigation nav) {
            Context.Settings.DetectJumps = DetectJumps;

            await Context.Storage.SaveSettings(Context.Settings);

            await nav.PopAsync();
        }

        public async Task DisconnectFromGoogle() {
            await Context.Security.ClearAccessToken();

            App.Current.MainPage = new LoginScreen(Context);
        }
    }
}
