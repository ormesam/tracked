using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Utilities;

namespace Tracked.Screens.Trails {
    public class TrailScreenViewModel : ViewModelBase {
        private TrailDto trail;

        public TrailScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => Trail.Name;

        public TrailDto Trail {
            get { return trail; }
            set {
                if (trail != value) {
                    trail = value;
                    OnPropertyChanged();
                }
            }
        }

        public MapControlViewModel MapViewModel { get; set; }

        public async Task Load(int id) {
            Trail = await Context.Services.GetTrail(id);

            MapViewModel = new MapControlViewModel(
                Context,
                Trail.Name,
                PolyUtils.GetMapLocations(Trail.Locations),
                showRideFeatures: false);

            OnPropertyChanged(nameof(MapViewModel));
        }

        public void ChangeName() {
            Context.UI.ShowInputDialog("Change Name", Trail.Name, async (newName) => {
                Trail.Name = newName;

                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Trail));

                await Context.Services.ChangeTrailName(Trail.TrailId.Value, newName);
            });
        }

        public async Task Delete() {
            await Context.Services.DeleteTrail(Trail.TrailId.Value);
        }
    }
}
