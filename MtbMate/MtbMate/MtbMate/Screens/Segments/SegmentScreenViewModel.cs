using MtbMate.Contexts;
using MtbMate.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MtbMate.Screens.Segments
{
    public class SegmentScreenViewModel : ViewModelBase
    {
        public SegmentModel Segment { get; }

        public SegmentScreenViewModel(MainContext context, SegmentModel segment) : base(context) {
            Segment = segment;
        }

        public override string Title => Segment.Name;

        public string DisplayName => Segment.DisplayName;

        public IList<LocationModel> Locations => Segment.Points
            .Select(i => new LocationModel {
                LatLong = i,
            })
            .ToList();

        public IList<SegmentAttemptModel> Attempts => Context.Model.SegmentAttempts
            .Where(i => i.SegmentId == Segment.Id)
            .ToList();

        public void ChangeName() {
            Context.UI.ShowInputDialog("Change Name", Segment.Name, async (newName) => {
                Segment.Name = newName;

                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DisplayName));

                await Context.Model.SaveSegment(Segment);
            });
        }

        public async Task GoToMapScreen(INavigation nav) {
            await Context.UI.GoToMapScreenAsync(nav, DisplayName, Segment.Points);
        }

        public async Task DeleteSegment(INavigation nav) {
            await Context.Model.RemoveSegment(Segment);

            await nav.PopAsync();
        }

        public async Task GoToAttempt(INavigation nav, SegmentAttemptModel attempt) {
            await Context.UI.GoToSegmentAttemptScreenAsync(nav, attempt);
        }
    }
}
