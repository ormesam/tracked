using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;

namespace MtbMate.Screens.Segments {
    public class SegmentScreenViewModel : ViewModelBase {
        public SegmentModel Segment { get; }

        public SegmentScreenViewModel(MainContext context, SegmentModel segment) : base(context) {
            Segment = segment;
        }

        public override string Title => Segment.Name;

        public string DisplayName => Segment.DisplayName;

        public IList<Location> Locations => Segment.Points
            .Select(i => new Location {
                LatLong = i,
            })
            .ToList();

        public IList<SegmentAttemptModel> Attempts => Model.Instance.SegmentAttempts
            .Where(i => i.SegmentId == Segment.Id)
            .OrderByDescending(i => i.Created)
            .ToList();

        public void ChangeName() {
            Context.UI.ShowInputDialog("Change Name", Segment.Name, async (newName) => {
                Segment.Name = newName;

                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DisplayName));

                await Model.Instance.SaveSegment(Segment);
            });
        }

        public async Task GoToMapScreen(INavigation nav) {
            await Context.UI.GoToMapScreenAsync(nav, DisplayName, Segment.Points);
        }

        public async Task DeleteSegment(INavigation nav) {
            await Model.Instance.RemoveSegment(Segment);

            await nav.PopAsync();
        }

        public async Task GoToAttempt(INavigation nav, SegmentAttemptModel attempt) {
            await Context.UI.GoToSegmentAttemptScreenAsync(nav, attempt);
        }

        public async Task RecompareRides() {
            await Model.Instance.RemoveSegmentAttempts(Attempts);

            await Model.Instance.AnalyseExistingRides(Segment);

            OnPropertyChanged(nameof(Attempts));
        }
    }
}
