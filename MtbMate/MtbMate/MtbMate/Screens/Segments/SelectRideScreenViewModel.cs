using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;
using Xamarin.Forms;

namespace MtbMate.Screens.Segments {
    public class SelectRideScreenViewModel : ViewModelBase {
        public SelectRideScreenViewModel(MainContext context) : base(context) {
        }

        public override string Title => "Create Segment";

        public ObservableCollection<Ride> Rides => Model.Instance.Rides
            .OrderByDescending(i => i.Start)
            .ToObservable();

        public async Task CreateSegment(INavigation nav, Ride ride) {
            await Context.UI.GoToCreateSegmentScreenAsync(nav, ride);
        }
    }
}
