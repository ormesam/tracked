using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Models;
using Tracked.Utilities;
using Xamarin.Forms;

namespace Tracked.Screens.Segments {
    public class SegmentScreenViewModel : ViewModelBase {
        public Segment Segment { get; }
        public MapControlViewModel MapViewModel { get; }

        public SegmentScreenViewModel(MainContext context, Segment segment) : base(context) {
            Segment = segment;
            MapViewModel = new MapControlViewModel(
                context,
                Segment.DisplayName,
                PolyUtils.GetMapLocations(Segment.Points),
                showRideFeatures: false);
        }

        public override string Title => Segment.Name;

        public string DisplayName => Segment.DisplayName;

        public IList<SegmentAttempt> Attempts => Model.Instance.SegmentAttempts
            .Where(i => i.SegmentId == Segment.Id)
            .OrderBy(i => i.Time)
            .ToList();

        public void ChangeName() {
            Context.UI.ShowInputDialog("Change Name", Segment.Name, async (newName) => {
                Segment.Name = newName;

                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DisplayName));

                await Model.Instance.SaveSegment(Segment);
            });
        }

        public async Task DeleteSegment(INavigation nav) {
            await Model.Instance.RemoveSegment(Segment);

            await nav.PopAsync();
        }

        public async Task GoToAttempt(SegmentAttempt attempt) {
            await Context.UI.GoToSegmentAttemptScreenAsync(attempt);
        }
    }
}
