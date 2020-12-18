using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Dtos;
using Shared.Interfaces;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Screens.Trails {
    public class TrailScreenViewModel : MapViewModelBase {
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

        public async Task Load(int id) {
            Trail = await Context.Services.GetTrail(id);

            OnPropertyChanged();
        }

        public async Task ChangeName() {
            string newName = await Context.UI.ShowPromptAsync("Change Name", null, Trail.Name);

            if (string.IsNullOrWhiteSpace(newName)) {
                return;
            }

            Trail.Name = newName;

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Trail));

            await Context.Services.ChangeTrailName(Trail.TrailId.Value, newName);
        }

        public async Task Delete() {
            await Context.Services.DeleteTrail(Trail.TrailId.Value);
        }
        protected override ILatLng Centre => Trail.Locations.Midpoint();

        protected override IEnumerable<MapPin> GetPins() {
            return new List<MapPin>();
        }

        protected override IEnumerable<MapPolyline> GetPolylines() {
            yield return new MapPolyline {
                Colour = Color.Blue,
                Width = 10,
                Positions = Trail.Locations,
            };
        }
    }
}
