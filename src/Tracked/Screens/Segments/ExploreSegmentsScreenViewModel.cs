using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Screens.Settings {
    public class ExploreSegmentsScreenViewModel : ViewModelBase {
        private bool isRefreshing;

        public ExploreSegmentsScreenViewModel(MainContext context) : base(context) {
            Segments = new List<SegmentOverviewDto>();
        }

        public override string Title => "Segments";

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

        public IList<SegmentOverviewDto> Segments { get; set; }

        public bool HasSegments => Segments.Any();

        public async Task Load() {
            IsRefreshing = true;

            try {
                Segments = await Context.Services.GetSegmentOverviews();
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            } finally {
                IsRefreshing = false;
                Refresh();
            }
        }

        public async Task AddSegment() {
            await Context.UI.GoToCreateSegmentScreenAsync();
        }

        public async Task GoToSegment(SegmentOverviewDto segment) {
            await Context.UI.GoToSegmentScreenAsync(segment.SegmentId);
        }
    }
}
