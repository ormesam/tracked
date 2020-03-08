using System.Threading.Tasks;
using Tracked.Contexts;
using Tracked.Models;

namespace Tracked.Screens.Segments {
    public class SelectRideScreenViewModel : ViewModelBase {
        public SelectRideScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => "Create Segment";

        //public ObservableCollection<Ride> Rides => Model.Instance.Rides
        //    .OrderByDescending(i => i.Start)
        //    .ToObservable();

        public async Task CreateSegment(Ride ride) {
            await Context.UI.GoToCreateSegmentScreenAsync(ride);
        }
    }
}
