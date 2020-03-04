using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Tracked.Contexts;
using Tracked.Models;
using Xamarin.Forms;

namespace Tracked.Screens.Settings {
    public class ExploreSegmentsScreenViewModel : ViewModelBase {
        public ExploreSegmentsScreenViewModel(MainContext context) : base(context) {
        }

        public ObservableCollection<Segment> Segments => Model.Instance.Segments;

        public override string Title => "Segments";

        public async Task AddSegment() {
            await Context.UI.GoToCreateSegmentScreenAsync();
        }

        public async Task GoToSegment(Segment segment) {
            await Context.UI.GoToSegmentScreenAsync(segment);
        }
    }
}
