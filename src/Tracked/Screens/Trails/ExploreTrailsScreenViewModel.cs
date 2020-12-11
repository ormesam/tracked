using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Screens.Trails {
    public class ExploreTrailsScreenViewModel : TabbedViewModelBase {
        private bool isRefreshing;

        public ExploreTrailsScreenViewModel(MainContext context) : base(context) {
            Trails = new List<TrailOverviewDto>();
        }

        public override string Title => "Trails";

        protected override TabItemType SelectedTab => TabItemType.Trails;

        public bool IsRefreshing {
            get { return isRefreshing; }
            set {
                if (value != isRefreshing) {
                    isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        public ICommand RefreshCommand {
            get { return new Command(async () => await Load()); }
        }

        public IList<TrailOverviewDto> Trails { get; set; }

        public bool HasTrails => Trails.Any();

        public bool CanCreateTrail => Context.Security.IsAdmin;

        public async Task Load() {
            IsRefreshing = true;

            try {
                Trails = await Context.Services.GetTrailOverviews();
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            } finally {
                IsRefreshing = false;

                OnPropertyChanged();
            }
        }

        public async Task AddTrail() {
            await Context.UI.GoToCreateTrailScreenAsync();
        }

        public async Task GoToTrail(TrailOverviewDto trail) {
            await Context.UI.GoToTrailScreenAsync(trail.TrailId);
        }
    }
}
