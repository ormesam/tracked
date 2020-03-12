using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Utilities;

namespace Tracked.Screens.SegmentAttempt {
    public class SegmentAttemptScreenViewModel : ViewModelBase {
        private SegmentAttemptDto segmentAttempt;

        public SegmentAttemptScreenViewModel(MainContext context) : base(context) {
        }

        public SegmentAttemptDto SegmentAttempt {
            get { return segmentAttempt; }
            set {
                if (segmentAttempt != value) {
                    segmentAttempt = value;
                    OnPropertyChanged();
                }
            }
        }

        public MapControlViewModel MapViewModel { get; set; }

        public async Task Load(int segmentAttemptId) {
            SegmentAttempt = await Context.Services.GetSegmentAttempt(segmentAttemptId);

            MapViewModel = new MapControlViewModel(
                Context,
                SegmentAttempt.FormattedTime,
                PolyUtils.GetMapLocations(SegmentAttempt.Locations, SegmentAttempt.Jumps));

            OnPropertyChanged(nameof(MapViewModel));
        }

        public async Task GoToSpeedAnalysis() {
            await Context.UI.GoToSpeedAnalysisScreenAsync(SegmentAttempt.Locations);
        }
    }
}
