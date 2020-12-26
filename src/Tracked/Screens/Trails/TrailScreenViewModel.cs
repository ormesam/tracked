using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> Load(int id) {
            try {
                Trail = await Context.Services.GetTrail(id);

                OnPropertyChanged();

                return true;
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);

                return false;
            }
        }

        public async Task ChangeName() {
            string newName = await Context.UI.ShowPromptAsync("Change Name", null, Trail.Name);

            if (string.IsNullOrWhiteSpace(newName)) {
                return;
            }

            Trail.Name = newName;

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Trail));

            try {
                await Context.Services.ChangeTrailName(Trail.TrailId.Value, newName);
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            }
        }

        public async Task Delete() {
            try {
                await Context.Services.DeleteTrail(Trail.TrailId.Value);
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            }
        }

        protected override ILatLng Centre => Trail.Locations.Midpoint();

        protected override IEnumerable<MapPin> GetPins() {
            return new List<MapPin>();
        }

        protected override IEnumerable<MapPolyline> GetPolylines() {
            yield return new MapPolyline {
                StrokeColor = Color.Blue,
                StrokeWidth = 10,
                Positions = Trail.Locations.ToList(),
            };
        }
    }
}
