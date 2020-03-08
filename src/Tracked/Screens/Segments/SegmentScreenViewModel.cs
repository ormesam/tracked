using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Utilities;

namespace Tracked.Screens.Segments {
    public class SegmentScreenViewModel : ViewModelBase {
        private SegmentDto segment;

        public SegmentScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => Segment.Name;

        public SegmentDto Segment {
            get { return segment; }
            set {
                if (segment != value) {
                    segment = value;
                    OnPropertyChanged();
                }
            }
        }

        public MapControlViewModel MapViewModel { get; set; }

        public async Task Load(int id) {
            Segment = await Context.Services.GetSegment(id);

            MapViewModel = new MapControlViewModel(
                Context,
                Segment.Name,
                PolyUtils.GetMapLocations(Segment.Locations),
                showRideFeatures: false);

            OnPropertyChanged(nameof(MapViewModel));
        }

        public IList<SegmentAttemptOverviewDto> Attempts => Segment.Attempts
            .OrderBy(i => i.Time)
            .ToList();

        public void ChangeName() {
            Context.UI.ShowInputDialog("Change Name", Segment.Name, async (newName) => {
                Segment.Name = newName;

                OnPropertyChanged(nameof(Title));

                // await Model.Instance.SaveSegment(Segment);
            });
        }

        public async Task GoToAttempt(SegmentAttemptOverviewDto attempt) {
            await Context.UI.GoToSegmentAttemptScreenAsync(attempt.SegmentAttemptId);
        }
    }
}
