using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;

namespace MtbMate.Screens.Settings {
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

        public async Task Save(Xamarin.Forms.INavigation nav) {
            Context.Settings.DetectJumps = DetectJumps;

            await Context.Storage.SaveSettings(Context.Settings);

            await nav.PopAsync();
        }

        public async Task DisconnectFromGoogle() {
            await Context.Security.ClearAccessToken();

            OnPropertyChanged();
        }

        public async Task Sync() {
            var ridesToSync = Model.Instance.Rides
                .Where(i => i.RideId == null);

            foreach (var ride in ridesToSync) {
                ride.RideId = await Context.Services.Sync(ride);

                await Model.Instance.SaveRide(ride);
            }
        }
    }
}
