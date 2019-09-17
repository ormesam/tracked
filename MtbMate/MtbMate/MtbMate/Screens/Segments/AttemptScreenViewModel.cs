using System.Collections.Generic;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using Xamarin.Forms;

namespace MtbMate.Screens.Segments {
    public class AttemptScreenViewModel : ViewModelBase {
        private readonly SegmentAttempt attempt;

        public AttemptScreenViewModel(MainContext context, SegmentAttempt attempt) : base(context) {
            this.attempt = attempt;
        }

        public override string Title => DisplayName;

        public IList<Location> Locations => attempt.Locations;
        public string DisplayName => attempt.Segment.DisplayName;
        public double AverageSpeed => attempt.AverageSpeed;
        public double MaxSpeed => attempt.MaxSpeed;
        public double Distance => attempt.Distance;
        public string Time => attempt.Time;

        public async Task GoToMapScreen(INavigation nav) {
            await Context.UI.GoToMapScreenAsync(nav, DisplayName, Locations);
        }
    }
}
